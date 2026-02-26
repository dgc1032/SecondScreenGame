const express = require('express');
const app = express();
const PORT = 3000;

let playerMoves = [null, null, null, null];
let playerSlots = [false, false, false, false];

app.use(express.static('public'));

app.get('/join', (req, res) => {
    const slot = playerSlots.findIndex(taken => !taken);

    if(slot === -1){
        return res.status(403).send("Game full");
    }

    playerSlots[slot] = true;
    res.send({playerID: slot});
});

app.get('/move', (req, res) => {
    const player = parseInt(req.query.player);
    const dir = req.query.dir;

    if(player >= 0 && player < 4 && ['up','down','left','right'].includes(dir)){
        playerMoves[player] = dir;
        res.sendStatus(200);
    } else {
        res.sendStatus(400);
    }
});

app.get('/getMoves', (req, res) => {
    res.json(playerMoves);
    playerMoves = [null, null, null, null]; // reset after sending
});

app.listen(PORT, () => console.log(`Server running on port ${PORT}`));