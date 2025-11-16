from fastapi import FastAPI
from pydantic import BaseModel
from model import ModelWrapper  # Still works in same folder

app = FastAPI(title="PyTorch ML Model API")

# Load model once
model = ModelWrapper(model_path="best_pr_testcase_model.pt")

class InputData(BaseModel):
    data: list

@app.get("/")
def root():
    return {"message": "ML Model API is running"}

@app.post("/predict")
def predict(input_data: InputData):
    predictions = model.predict(input_data.data)
    return {"predictions": predictions}
