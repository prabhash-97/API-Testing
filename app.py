from fastapi import FastAPI
from model import predict_testcases

app = FastAPI(title="ML PR Test Case API")

@app.get("/")
def root():
    return {"message": "API running on Hugging Face"}

@app.get("/predict")
def predict(pr_title: str, top_k: int = 5):
    results = predict_testcases(pr_title, top_k)
    return {"predictions": results.to_dict(orient="records")}
