// server.js - Node.js local server to read data_logs/*.json

const express = require('express');
const fs = require('fs');
const path = require('path');
const app = express();
const PORT = 3000;

const DATA_DIR = '/Users/andy/Work/1132/BeeTrap/BeeTrapSocket/BeeTrapReceiver/data_logs';

// Serve frontend files
app.use(express.static('public'));

// API: list all JSON files
app.get('/api/files', (req, res) => {
  fs.readdir(DATA_DIR, (err, files) => {
    if (err) return res.status(500).send('Error reading directory');
    const jsonFiles = files.filter(f => f.endsWith('.json'));
    res.json(jsonFiles);
  });
});

// API: get one JSON file content
app.get('/api/file/:name', (req, res) => {
  const filePath = path.join(DATA_DIR, req.params.name);
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) return res.status(404).send('File not found');
    res.type('json').send(data);
  });
});

app.listen(PORT, () => {
  console.log(`🟢 JSON Viewer Server running at http://localhost:${PORT}`);
});
