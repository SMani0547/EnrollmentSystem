import urllib
from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()

def init_app(app):
    params = urllib.parse.quote_plus(
        "DRIVER={ODBC Driver 17 for SQL Server};"
        "SERVER=FJ241064;"
        "DATABASE=USPGradeSystemDB;"
        "UID=sa;"
        "PWD=hassql;"
    )

    app.config['SQLALCHEMY_DATABASE_URI'] = f"mssql+pyodbc:///?odbc_connect={params}"
    app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
    db.init_app(app)

