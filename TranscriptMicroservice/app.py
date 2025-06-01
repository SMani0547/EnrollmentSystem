from flask import Flask, request, jsonify, send_file
from flask_cors import CORS
from database import init_app, db
from models import Grade
from transcript_generator import generate_pdf
from auth import auth
import os

app = Flask(__name__)
CORS(app)  # Allow cross-origin requests
init_app(app)

# Ensure tables exist
with app.app_context():
    db.create_all()

@app.route("/transcript", methods=["POST"])
@auth.login_required
def post_transcript_request():
    student_id = request.json.get("StudentId")
    if not student_id:
        return jsonify({"error": "StudentId is required"}), 400
    return jsonify({"message": "Transcript request received", "StudentId": student_id})

@app.route("/transcript/<student_id>", methods=["GET"])
@auth.login_required
def get_transcript(student_id):
    try:
        records = Grade.query.filter_by(student_id=student_id).all()
        if not records:
            return jsonify({"error": "No transcript found for this StudentId"}), 404

        pdf_file = generate_pdf(records, student_id)
        if not pdf_file or not os.path.exists(pdf_file):
            return jsonify({"error": "Failed to generate PDF"}), 500

        return send_file(pdf_file, as_attachment=True)
    except Exception as e:
        return jsonify({"error": f"Internal server error: {str(e)}"}), 500

if __name__ == "__main__":
    app.run(host='127.0.0.1', port=5240, debug=True)
