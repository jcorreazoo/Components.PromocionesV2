---
name: pref-preguntas-de-a-una
description: The user prefers questions asked one at a time, not several at once
metadata:
  type: feedback
---

When asking the user clarifying questions (including via AskUserQuestion), ask **one question at a time**, not several batched together.

**Why:** Batching multiple questions at once is easy to mis-handle (the user accidentally dismissed a 3-question prompt) and harder to answer thoughtfully.

**How to apply:** Pose a single question, wait for the answer, then move to the next. If using AskUserQuestion, send only one question per call. Related: [[estado-actual]].
