from fpdf import FPDF
import os
from datetime import datetime
import uuid

def generate_pdf(transcript_data, student_id):
    pdf = FPDF()
    pdf.add_page()
    pdf.set_auto_page_break(auto=True, margin=15)

    # Get the directory where this script is located
    script_dir = os.path.dirname(os.path.abspath(__file__))
    
    # Add header image
    header1_path = os.path.join(script_dir, "images", "header_1.png")
    print(f"Looking for header image at: {header1_path}")
    
    if os.path.exists(header1_path):
        try:
            # Position header image with smaller size
            pdf.image(header1_path, x=10, y=10, w=100)  # Reduced width to 100mm
            print("Header image added successfully")
        except Exception as e:
            print(f"Error adding header image: {str(e)}")
    else:
        print(f"Header image not found at: {header1_path}")

    # Add space after header
    pdf.ln(60)  # Reduced spacing to make room for footer

    # Title and Student Information
    pdf.set_font("Arial", style='B', size=16)  # Increased size
    pdf.set_text_color(0, 0, 0)  # Changed to black
    pdf.cell(190, 10, txt=f"Academic Transcript", ln=True, align='C')
    pdf.ln(5)  # Small space after title
    
    # Student Details (left-aligned)
    pdf.set_font("Arial", style='B', size=11)
    pdf.set_text_color(0, 0, 0)
    pdf.cell(60, 8, "Student ID:", ln=0)  # Reduced height
    pdf.set_font("Arial", size=11)
    pdf.cell(130, 8, str(student_id), ln=True)  # Reduced height
    
    # Add date (without time)
    current_date = datetime.now().strftime("%Y-%m-%d")
    pdf.set_font("Arial", style='B', size=11)
    pdf.cell(60, 8, "Date:", ln=0)  # Reduced height
    pdf.set_font("Arial", size=11)
    pdf.cell(130, 8, current_date, ln=True)  # Reduced height
    pdf.ln(8)  # Reduced space before grade table

    # Column headers with background color (USP theme color)
    pdf.set_fill_color(0, 147, 151)     # #009397 in RGB
    pdf.set_text_color(255, 255, 255)   # White text
    pdf.set_font("Arial", style='B', size=11)
    pdf.cell(80, 8, "Course ID", border=1, fill=True, align='C')  # Reduced height
    pdf.cell(80, 8, "Marks", border=1, fill=True, align='C')  # Reduced height
    pdf.cell(30, 8, "Grade", border=1, fill=True, align='C')  # Reduced height
    pdf.ln()

    # Table body
    pdf.set_font("Arial", size=11)
    pdf.set_text_color(0, 0, 0)  # Reset text to black
    fill = False  # Used to alternate row background

    for record in transcript_data:
        pdf.set_fill_color(230, 240, 255)  # Light blue rows
        pdf.cell(80, 8, str(record.course_id), border=1, fill=fill, align='C')  # Reduced height
        pdf.cell(80, 8, str(record.marks), border=1, fill=fill, align='C')  # Reduced height
        pdf.cell(30, 8, str(record.grade_letter), border=1, fill=fill, align='C')  # Reduced height
        pdf.ln()
        fill = not fill  # Alternate background color

    # Add footer image immediately after the table
    footer_path = os.path.join(script_dir, "images", "GradeFooter.png")
    print(f"Looking for footer image at: {footer_path}")
    print(f"Footer image exists: {os.path.exists(footer_path)}")
    
    if os.path.exists(footer_path):
        try:
            pdf.ln(10)  # Small space after table
            pdf.image(footer_path, x=10, y=None, w=190)  # Full width
            print("Footer image added successfully")
        except Exception as e:
            print(f"Error adding footer image: {str(e)}")
    else:
        print(f"Footer image not found at: {footer_path}")

    # Save file
    output_dir = "generated_transcripts"
    os.makedirs(output_dir, exist_ok=True)
    file_path = os.path.join(output_dir, f"transcript_{student_id}.pdf")
    pdf.output(file_path)

    return file_path if os.path.exists(file_path) else None
