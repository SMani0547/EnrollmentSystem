from fpdf import FPDF
import os

def generate_pdf(transcript_data, student_id):
    pdf = FPDF()
    pdf.add_page()
    pdf.set_auto_page_break(auto=True, margin=15)

    # Header
    pdf.set_font("Arial", style='B', size=14)
    pdf.set_text_color(0, 51, 102)  # USP-themed dark blue
    pdf.cell(190, 10, txt=f"Academic Transcript - Student ID: {student_id}", ln=True, align='C')
    pdf.ln(5)

    # Column headers with background color
    pdf.set_fill_color(0, 102, 153)     # Dark teal blue
    pdf.set_text_color(255, 255, 255)   # White text
    pdf.set_font("Arial", style='B', size=12)
    pdf.cell(60, 10, "Course ID", border=1, fill=True, align='C')
    pdf.cell(40, 10, "Marks", border=1, fill=True, align='C')
    pdf.cell(40, 10, "Grade", border=1, fill=True, align='C')
    pdf.ln()

    # Table body
    pdf.set_font("Arial", size=11)
    pdf.set_text_color(0, 0, 0)  # Reset text to black
    fill = False  # Used to alternate row background

    for record in transcript_data:
        pdf.set_fill_color(230, 240, 255)  # Light blue rows
        pdf.cell(60, 10, str(record.course_id), border=1, fill=fill, align='C')
        pdf.cell(40, 10, str(record.marks), border=1, fill=fill, align='C')
        pdf.cell(40, 10, str(record.grade_letter), border=1, fill=fill, align='C')
        pdf.ln()
        fill = not fill  # Alternate background color

    # Save file
    output_dir = "generated_transcripts"
    os.makedirs(output_dir, exist_ok=True)
    file_path = os.path.join(output_dir, f"transcript_{student_id}.pdf")
    pdf.output(file_path)

    return file_path if os.path.exists(file_path) else None
