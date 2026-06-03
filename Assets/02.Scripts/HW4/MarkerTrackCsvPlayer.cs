using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Video;

public class MarkerTrackCsvPlayer : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;
    public Transform sceneCenter;
    public Transform markerAnchor;

    [Header("Marker Content")]
    public GameObject markerContent;

    [Header("CSV Resource Path")]
    [Tooltip("Do not include .csv extension. Example: MarkerTracking/marker_track")]
    public string csvResourcePath = "MarkerTracking/marker_track";

    [Header("Marker Selection")]
    public int targetMarkerId = 17;

    [Header("Placement")]
    [Tooltip("Distance from sceneCenter to markerAnchor. Usually slightly smaller than VideoSphere radius.")]
    public float markerDistance = 80.0f;

    [Tooltip("Additional vertical offset after direction mapping.")]
    public float verticalOffset = 0.0f;

    [Header("360 Mapping Calibration")]
    public bool invertYaw = false;
    public bool invertPitch = false;
    public float yawOffsetDegrees = 0.0f;
    public float pitchOffsetDegrees = 0.0f;

    [Header("Lock Mode")]
    [Tooltip("If true, the anchor uses one marker sample and never moves again.")]
    public bool lockToFirstDetection = true;

    [Tooltip("If true, use the first CSV sample immediately when the scene starts.")]
    public bool useFirstCsvSampleImmediately = true;

    [Tooltip("Used only when useFirstCsvSampleImmediately is false.")]
    public float maxTimeGap = 1.0f;

    [Header("Debug")]
    public bool printDebugLog = true;

    private readonly List<MarkerSample> samples = new List<MarkerSample>();

    private bool hasLockedPose = false;
    private Vector3 lockedPosition;
    private Quaternion lockedRotation;

    [Serializable]
    private class MarkerSample
    {
        public int frame;
        public float time;
        public int markerId;
        public float centerX;
        public float centerY;
        public float yaw;
        public float pitch;
    }

    private void Start()
    {
        LoadCsv();

        if (markerContent != null)
        {
            markerContent.SetActive(false);
        }

        if (lockToFirstDetection && useFirstCsvSampleImmediately)
        {
            TryLockToFirstSample();
        }
    }

    private void Update()
    {
        if (markerAnchor == null || sceneCenter == null)
        {
            return;
        }

        if (lockToFirstDetection && hasLockedPose)
        {
            markerAnchor.position = lockedPosition;
            markerAnchor.rotation = lockedRotation;

            if (markerContent != null)
            {
                markerContent.SetActive(true);
            }

            return;
        }

        if (lockToFirstDetection)
        {
            TryLockByCurrentVideoTime();
            return;
        }

        UpdateContinuouslyByVideoTime();
    }

    private void LoadCsv()
    {
        samples.Clear();

        TextAsset csv = Resources.Load<TextAsset>(csvResourcePath);

        if (csv == null)
        {
            Debug.LogError("[MarkerTrackCsvPlayer] CSV not found. Check path: Resources/" + csvResourcePath + ".csv");
            return;
        }

        string[] lines = csv.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 1)
        {
            Debug.LogError("[MarkerTrackCsvPlayer] CSV has no data.");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            string[] cols = line.Split(',');

            // Expected header:
            // frame,time,marker_id,center_x,center_y,yaw,pitch
            if (cols.Length < 7)
            {
                continue;
            }

            try
            {
                int markerId = int.Parse(cols[2], CultureInfo.InvariantCulture);

                if (markerId != targetMarkerId)
                {
                    continue;
                }

                MarkerSample sample = new MarkerSample();
                sample.frame = int.Parse(cols[0], CultureInfo.InvariantCulture);
                sample.time = float.Parse(cols[1], CultureInfo.InvariantCulture);
                sample.markerId = markerId;
                sample.centerX = float.Parse(cols[3], CultureInfo.InvariantCulture);
                sample.centerY = float.Parse(cols[4], CultureInfo.InvariantCulture);
                sample.yaw = float.Parse(cols[5], CultureInfo.InvariantCulture);
                sample.pitch = float.Parse(cols[6], CultureInfo.InvariantCulture);

                samples.Add(sample);
            }
            catch (Exception e)
            {
                Debug.LogWarning("[MarkerTrackCsvPlayer] Failed to parse CSV line: " + line + "\n" + e.Message);
            }
        }

        samples.Sort((a, b) => a.time.CompareTo(b.time));

        if (printDebugLog)
        {
            Debug.Log("[MarkerTrackCsvPlayer] Loaded samples for marker ID " + targetMarkerId + ": " + samples.Count);

            if (samples.Count > 0)
            {
                MarkerSample first = samples[0];
                Debug.Log(
                    "[MarkerTrackCsvPlayer] First sample: " +
                    "frame=" + first.frame +
                    ", time=" + first.time +
                    ", id=" + first.markerId +
                    ", yaw=" + first.yaw +
                    ", pitch=" + first.pitch
                );
            }
        }
    }

    private void TryLockToFirstSample()
    {
        if (samples.Count == 0)
        {
            Debug.LogWarning("[MarkerTrackCsvPlayer] No marker samples found for targetMarkerId=" + targetMarkerId);
            return;
        }

        MarkerSample sample = samples[0];
        ApplySample(sample, true);
    }

    private void TryLockByCurrentVideoTime()
    {
        if (videoPlayer == null)
        {
            return;
        }

        MarkerSample sample = FindNearestSample((float)videoPlayer.time);

        if (sample == null)
        {
            if (markerContent != null)
            {
                markerContent.SetActive(false);
            }

            return;
        }

        float timeGap = Mathf.Abs(sample.time - (float)videoPlayer.time);

        if (timeGap > maxTimeGap)
        {
            if (markerContent != null)
            {
                markerContent.SetActive(false);
            }

            return;
        }

        ApplySample(sample, true);
    }

    private void UpdateContinuouslyByVideoTime()
    {
        if (videoPlayer == null)
        {
            return;
        }

        MarkerSample sample = FindNearestSample((float)videoPlayer.time);

        if (sample == null)
        {
            if (markerContent != null)
            {
                markerContent.SetActive(false);
            }

            return;
        }

        float timeGap = Mathf.Abs(sample.time - (float)videoPlayer.time);

        if (timeGap > maxTimeGap)
        {
            if (markerContent != null)
            {
                markerContent.SetActive(false);
            }

            return;
        }

        ApplySample(sample, false);
    }

    private void ApplySample(MarkerSample sample, bool lockPose)
    {
        float calibratedYaw = sample.yaw;
        float calibratedPitch = sample.pitch;

        if (invertYaw)
        {
            calibratedYaw = -calibratedYaw;
        }

        if (invertPitch)
        {
            calibratedPitch = -calibratedPitch;
        }

        calibratedYaw += yawOffsetDegrees;
        calibratedPitch += pitchOffsetDegrees;

        Vector3 direction = YawPitchToDirection(calibratedYaw, calibratedPitch);

        Vector3 center = sceneCenter.position;
        Vector3 targetPosition = center + direction * markerDistance;
        targetPosition.y += verticalOffset;

        markerAnchor.position = targetPosition;

        // Make the anchor face the inside of the 360 sphere.
        Vector3 lookDirection = center - markerAnchor.position;

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            markerAnchor.rotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);
        }

        if (markerContent != null)
        {
            markerContent.SetActive(true);
        }

        if (lockPose)
        {
            lockedPosition = markerAnchor.position;
            lockedRotation = markerAnchor.rotation;
            hasLockedPose = true;
        }

        if (printDebugLog)
        {
            Debug.Log(
                "[MarkerTrackCsvPlayer] Applied marker sample." +
                " frame=" + sample.frame +
                ", time=" + sample.time +
                ", id=" + sample.markerId +
                ", rawYaw=" + sample.yaw +
                ", rawPitch=" + sample.pitch +
                ", calibratedYaw=" + calibratedYaw +
                ", calibratedPitch=" + calibratedPitch +
                ", position=" + markerAnchor.position +
                ", rotation=" + markerAnchor.rotation.eulerAngles +
                ", locked=" + hasLockedPose
            );
        }
    }

    private MarkerSample FindNearestSample(float time)
    {
        if (samples.Count == 0)
        {
            return null;
        }

        int left = 0;
        int right = samples.Count - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;

            if (samples[mid].time < time)
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }

        MarkerSample best = samples[left];

        if (left > 0)
        {
            MarkerSample previous = samples[left - 1];

            if (Mathf.Abs(previous.time - time) < Mathf.Abs(best.time - time))
            {
                best = previous;
            }
        }

        return best;
    }

    private Vector3 YawPitchToDirection(float yawDeg, float pitchDeg)
    {
        float yaw = yawDeg * Mathf.Deg2Rad;
        float pitch = pitchDeg * Mathf.Deg2Rad;

        float x = Mathf.Cos(pitch) * Mathf.Sin(yaw);
        float y = Mathf.Sin(pitch);
        float z = Mathf.Cos(pitch) * Mathf.Cos(yaw);

        return new Vector3(x, y, z).normalized;
    }

    [ContextMenu("Reload CSV And Lock First Sample")]
    private void ReloadCsvAndLockFirstSample()
    {
        hasLockedPose = false;
        LoadCsv();
        TryLockToFirstSample();
    }

    [ContextMenu("Clear Locked Pose")]
    private void ClearLockedPose()
    {
        hasLockedPose = false;

        if (markerContent != null)
        {
            markerContent.SetActive(false);
        }

        Debug.Log("[MarkerTrackCsvPlayer] Locked pose cleared.");
    }
}