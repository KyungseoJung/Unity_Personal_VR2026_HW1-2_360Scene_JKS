import cv2
import numpy as np
import csv
import math
import os

VIDEO_PATH = "video360.mp4"
OUTPUT_CSV = "marker_track.csv"

# ArUco marker dictionary.
# 네 marker가 다른 종류라면 여기만 바꾸면 됨.
ARUCO_DICT_NAME = cv2.aruco.DICT_4X4_50

# 모든 프레임을 검사하면 느릴 수 있으므로 n프레임마다 검사.
FRAME_STEP = 5

def pixel_to_yaw_pitch(u, v, width, height):
    """
    Equirectangular 360 video pixel coordinate -> yaw/pitch angle.
    yaw: left/right direction in degrees
    pitch: up/down direction in degrees
    """
    yaw = (u / width) * 360.0 - 180.0
    pitch = 90.0 - (v / height) * 180.0
    return yaw, pitch

def main():
    if not os.path.exists(VIDEO_PATH):
        raise FileNotFoundError(f"Video not found: {VIDEO_PATH}")

    cap = cv2.VideoCapture(VIDEO_PATH)

    if not cap.isOpened():
        raise RuntimeError("Failed to open video.")

    fps = cap.get(cv2.CAP_PROP_FPS)
    width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
    height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
    total_frames = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))

    print(f"Video: {VIDEO_PATH}")
    print(f"Resolution: {width} x {height}")
    print(f"FPS: {fps}")
    print(f"Total frames: {total_frames}")

    aruco_dict = cv2.aruco.getPredefinedDictionary(ARUCO_DICT_NAME)
    parameters = cv2.aruco.DetectorParameters()
    detector = cv2.aruco.ArucoDetector(aruco_dict, parameters)

    rows = []

    frame_index = 0

    while True:
        ret, frame = cap.read()
        if not ret:
            break

        if frame_index % FRAME_STEP != 0:
            frame_index += 1
            continue

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        corners, ids, rejected = detector.detectMarkers(gray)

        time_sec = frame_index / fps if fps > 0 else 0.0

        if ids is not None:
            for marker_corners, marker_id in zip(corners, ids.flatten()):
                pts = marker_corners[0]

                center_x = float(np.mean(pts[:, 0]))
                center_y = float(np.mean(pts[:, 1]))

                yaw, pitch = pixel_to_yaw_pitch(center_x, center_y, width, height)

                rows.append({
                    "frame": frame_index,
                    "time": time_sec,
                    "marker_id": int(marker_id),
                    "center_x": center_x,
                    "center_y": center_y,
                    "yaw": yaw,
                    "pitch": pitch
                })

                print(
                    f"frame={frame_index}, time={time_sec:.2f}, "
                    f"id={marker_id}, center=({center_x:.1f},{center_y:.1f}), "
                    f"yaw={yaw:.2f}, pitch={pitch:.2f}"
                )

        frame_index += 1

    cap.release()

    with open(OUTPUT_CSV, "w", newline="", encoding="utf-8") as f:
        fieldnames = ["frame", "time", "marker_id", "center_x", "center_y", "yaw", "pitch"]
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(rows)

    print(f"Saved: {OUTPUT_CSV}")
    print(f"Detected rows: {len(rows)}")

if __name__ == "__main__":
    main()