import random
import time
import string
from datetime import datetime

from flask import Flask, jsonify, request

app = Flask(__name__)


def generate_transaction_id():
    """Generate a random transaction ID"""
    return "TX" + "".join(random.choices(string.digits, k=8))


@app.route("/process-payment", methods=["POST"])
def process_payment():
    try:
        # Get JSON data from request
        data = request.get_json()

        # Validate required fields
        required_fields = ["amount", "currency", "description", "referenceId"]
        for field in required_fields:
            if field not in data:
                return (
                    jsonify(
                        {
                            "status": "Error",
                            "message": f"Missing required field: {field}",
                            "timestamp": datetime.utcnow().isoformat() + "Z",
                        }
                    ),
                    400,
                )

        time.sleep(0.3)  # Simulate processing delay

        # Simulate payment processing
        response = {
            "status": "Success",
            "transactionId": generate_transaction_id(),
            "timestamp": datetime.utcnow().isoformat() + "Z",
            "message": "Payment processed successfully",
            "referenceId": data["referenceId"],
        }

        return jsonify(response), 200

    except Exception as e:
        print(f"Error processing payment: {e}")
        return (
            jsonify(
                {
                    "status": "Error",
                    "message": "Internal server error",
                    "timestamp": datetime.utcnow().isoformat() + "Z",
                }
            ),
            500,
        )


if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0", port=5000)
