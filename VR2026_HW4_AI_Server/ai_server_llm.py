from http.server import BaseHTTPRequestHandler, HTTPServer
import json
import urllib.request
import urllib.error


HOST = "127.0.0.1"
PORT = 8000

OLLAMA_URL = "http://127.0.0.1:11434/api/generate"
OLLAMA_MODEL = "llama3.2:3b"


def rule_based_fallback(girl_data):
    situation = girl_data.get("situation", "")

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


def build_prompt(girl_data):
    voice = girl_data.get("voice", "")
    action = girl_data.get("action", "")
    situation = girl_data.get("situation", "")

    prompt = f"""
You are Avatar B in a VR blind date scene.

Persona:
You are a friendly, slightly shy college student in your early 20s.
You respond politely and emotionally.
You are not too talkative. Keep your response short.

Avatar A just acted as follows:
- voice: {voice}
- action: {action}
- situation: {situation}

Return only one JSON object.
Do not include explanations.
Do not include markdown.
Do not include code blocks.

The JSON must have exactly these fields:
- voice
- action
- expression
- emotion

Allowed action values:
Bow, WaveHand, Talk, Laugh, Frown, Angry, Idle

Allowed expression values:
Happy, HappyTalk, Angry, Talk, Neutral

Allowed emotion values:
Positive, Negative, Neutral

Example:
{{
  "voice": "Nice to meet you too.",
  "action": "Bow",
  "expression": "Happy",
  "emotion": "Positive"
}}
"""
    return prompt.strip()


def call_ollama(girl_data):
    prompt = build_prompt(girl_data)

    request_data = {
        "model": OLLAMA_MODEL,
        "prompt": prompt,
        "stream": False,
        "format": "json",
        "options": {
            "temperature": 0.4
        }
    }

    body = json.dumps(request_data).encode("utf-8")

    request = urllib.request.Request(
        OLLAMA_URL,
        data=body,
        headers={"Content-Type": "application/json"},
        method="POST"
    )

    with urllib.request.urlopen(request, timeout=60) as response:
        response_body = response.read().decode("utf-8")
        ollama_result = json.loads(response_body)

    response_text = ollama_result.get("response", "").strip()
    boy_response = json.loads(response_text)

    return validate_boy_response(boy_response)


def validate_boy_response(response):
    allowed_actions = {"Bow", "WaveHand", "Talk", "Laugh", "Frown", "Angry", "Idle"}
    allowed_expressions = {"Happy", "HappyTalk", "Angry", "Talk", "Neutral"}
    allowed_emotions = {"Positive", "Negative", "Neutral"}

    voice = str(response.get("voice", "..."))
    action = str(response.get("action", "Idle"))
    expression = str(response.get("expression", "Neutral"))
    emotion = str(response.get("emotion", "Neutral"))

    if action not in allowed_actions:
        action = "Idle"

    if expression not in allowed_expressions:
        expression = "Neutral"

    if emotion not in allowed_emotions:
        emotion = "Neutral"

    return {
        "voice": voice,
        "action": action,
        "expression": expression,
        "emotion": emotion
    }


def generate_boy_response(girl_data):
    print("[Received GirlActionData]")
    print(
        f"voice={girl_data.get('voice', '')}, "
        f"action={girl_data.get('action', '')}, "
        f"situation={girl_data.get('situation', '')}"
    )

    try:
        boy_response = call_ollama(girl_data)
        print("[LLM Response]")
        print(boy_response)
        return boy_response

    except Exception as e:
        print("[LLM Error]")
        print(e)
        print("[Fallback] Using rule-based response.")
        return rule_based_fallback(girl_data)


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
    print(f"LLM AI server running at http://{HOST}:{PORT}/ai")
    print(f"Ollama model: {OLLAMA_MODEL}")
    print("Press Ctrl+C to stop the server.")
    server.serve_forever()


if __name__ == "__main__":
    run_server()