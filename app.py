from fastapi import FastAPI
from pydantic import BaseModel
from model import predict_testcases  # your ML code

app = FastAPI(title="PR Test Case Prioritization API")

# ✅ Define input model first
class PRInput(BaseModel):
    pr_title: str
    top_k: int = 5

# ✅ Then define endpoint
@app.post("/predict")
def predict(pr: PRInput):
    results = predict_testcases(pr.pr_title, pr.top_k)
    return {"predictions": results}  # return list directly
