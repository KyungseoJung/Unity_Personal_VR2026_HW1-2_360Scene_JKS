from http.server import BaseHTTPRequestHandler, HTTPServer
import json


HOST = "127.0.0.1"
PORT = 8000


def generate_boy_response(girl_data):
    situation = girl_data.get("situation", "")
    voice = girl_data.get("voice", "")
    action = girl_data.get("action", "")

    print("[Received GirlActionData]")
    print(f"voice={voice}, action={action}, situation={situation}")

    if situation == "FirstMeeting":
        return {
            "voice": "Nice to meet you too.",
            "action": "Bow",
            "expression": "Happy",
            "emotion": "Positive"
        }

    if situation == "Greeting":
        return {
            "voice": "Hello.",
            "action": "WaveHand",
            "expression": "HappyTalk",
            "emotion": "Positive"
        }

    if situation == "Farewell":
        return {
            "voice": "Goodbye.",
            "action": "WaveHand",
            "expression": "HappyTalk",
            "emotion": "Neutral"
        }

    if situation == "GoodJoke":
        return {
            "voice": "That was funny!",
            "action": "Laugh",
            "expression": "Happy",
            "emotion": "Positive"
        }

    if situation == "BadJoke":
        return {
            "voice": "That was awkward.",
            "action": "Frown",
            "expression": "Angry",
            "emotion": "Negative"
        }

    return {
        "voice": "...",
        "action": "Idle",
        "expression": "Neutral",
        "emotion": "Neutral"
    }


class AIRequestHandler(BaseHTTPRequestHandler):
    def do_POST(self):
        if self.path != "/ai":
            self.send_response(404)
            self.end_headers()
            self.wfile.write(b"Not Found")
            return

        content_length = int(self.headers.get("Content-Length", 0))
        body = self.rfile.read(content_length)

        try:
            girl_data = json.loads(body.decode("utf-8"))
            boy_response = generate_boy_response(girl_data)

            response_json = json.dumps(boy_response).encode("utf-8")

            self.send_response(200)
            self.send_header("Content-Type", "application/json")
            self.send_header("Content-Length", str(len(response_json)))
            self.end_headers()
            self.wfile.write(response_json)

            print("[Sent BoyResponseData]")
            print(boy_response)
            print()

        except Exception as e:
            error_response = {
                "error": str(e)
            }
            response_json = json.dumps(error_response).encode("utf-8")

            self.send_response(500)
            self.send_header("Content-Type", "application/json")
            self.send_header("Content-Length", str(len(response_json)))
            self.end_headers()
            self.wfile.write(response_json)


def run_server():
    server = HTTPServer((HOST, PORT), AIRequestHandler)
    print(f"AI server running at http://{HOST}:{PORT}/ai")
    print("Press Ctrl+C to stop the server.")
    server.serve_forever()


if __name__ == "__main__":
    run_server()