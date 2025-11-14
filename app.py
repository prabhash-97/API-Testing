import torch
from fastapi import FastAPI
from pydantic import BaseModel

model = torch.load("best_pr_testcase_model.pt", map_location="cpu")
model.eval()

app = FastAPI()

class Input(BaseModel):
    title: str
    body: str

@app.post("/predict")
def predict(input: Input):
    # dummy example — replace with real preprocess
    x = torch.tensor([len(input.title), len(input.body)], dtype=torch.float32)
    with torch.no_grad():
        out = model(x).tolist()
    return {"prediction": out}
