#!/bin/bash
NAMESPACE="${1:-codebase_b1540_app}"
docker build -t "$NAMESPACE" .