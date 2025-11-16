import torch
import numpy as np
import random

# =========================
# Config
# =========================
MAX_LEN = 50
VOCAB_SIZE = 1000
NUM_TCS = 50
DEVICE = torch.device("cuda" if torch.cuda.is_available() else "cpu")

# =========================
# Test case list
# =========================
all_tcs = [f"TC_{i}" for i in range(NUM_TCS)]

# =========================
# Model Architecture
# =========================
class SmallPRClassifier(torch.nn.Module):
    def __init__(self, vocab_size=VOCAB_SIZE, embed_dim=64, num_labels=NUM_TCS):
        super().__init__()
        self.embedding = torch.nn.EmbeddingBag(vocab_size, embed_dim)
        self.fc1 = torch.nn.Linear(embed_dim, 128)
        self.relu = torch.nn.ReLU()
        self.dropout = torch.nn.Dropout(0.3)
        self.classifier = torch.nn.Linear(128, num_labels)

    def forward(self, x):
        embedded = self.embedding(x)
        hidden = self.relu(self.fc1(embedded))
        logits = self.classifier(self.dropout(hidden))
        return logits

# =========================
# Tokenizer (dummy)
# =========================
def text_to_indices(text, max_len=MAX_LEN, vocab_size=VOCAB_SIZE):
    idxs = [random.randint(2, vocab_size-1) for _ in text.split()]
    if len(idxs) < max_len:
        idxs += [0]*(max_len - len(idxs))
    else:
        idxs = idxs[:max_len]
    return idxs

# =========================
# Load model
# =========================
model = SmallPRClassifier().to(DEVICE)
model.load_state_dict(torch.load("dummy_small_model.pt", map_location=DEVICE))
model.eval()

# =========================
# Prediction function
# =========================
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
