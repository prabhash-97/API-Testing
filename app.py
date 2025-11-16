from fastapi import FastAPI
from pydantic import BaseModel
from model import predict_testcases

app = FastAPI(title="PR Test Case Prioritization API")

class PRInput(BaseModel):
    pr_title: str
    top_k: int = 5

@app.get("/")
def root():
    return {"message": "ML API is running"}

@app.post("/predict")
def predict(pr: PRInput):
    results = predict_testcases(pr.pr_title, pr.top_k)
    return {"predictions": results}
