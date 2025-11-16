# model.py
import torch
import torch.nn as nn
import numpy as np
from main_code_above import SmallPRClassifier, text_to_indices, all_tcs  # or copy definitions here

DEVICE = torch.device("cuda" if torch.cuda.is_available() else "cpu")

# Load model
model = SmallPRClassifier().to(DEVICE)
model.load_state_dict(torch.load("best_pr_testcase_model.pt", map_location=DEVICE))
model.eval()

def predict_testcases(pr_title, top_k=5):
    idxs = torch.tensor([text_to_indices(pr_title)]).to(DEVICE)
    with torch.no_grad():
        logits = model(idxs)
        scores = torch.sigmoid(logits).cpu().numpy().flatten()
    ranked_indices = np.argsort(scores)[::-1][:top_k]
    results = []
    for idx in ranked_indices:
        results.append({
            "Test_Case_ID": all_tcs[idx],
            "Priority_Score": float(scores[idx])
        })
    return results
