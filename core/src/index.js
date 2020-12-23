const directoryPath = "resources";
const mapsFileName = "maps-info.json"


const express = require('express');
const bodyParser = require('body-parser');
const path = require('path');
const fs = require('fs');

const World = require('./world')

var jsonParser = bodyParser.json();


maps = JSON.parse(fs.readFileSync(path.join(directoryPath, mapsFileName)))

var app = express();

app.use(express.static(__dirname + "/public"));


app.get("/health", function (req, res) {
    res.send("Server is working!");
});

app.get("/get", function (req, res) {
    res.send(maps);
})

app.get("/get/:id", function (req, res) {
    var id = req.params.id;
    try {
        var map = fs.readFileSync(path.join(directoryPath, "map" + id)).toString();
        res.send(map);
    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        }
    }
})

function buildMapFromJSON(mapEncoded) {
    const cellByNum = {
        1: World.CELL.ROCK,
        2: World.CELL.RAVINE,
        3: World.CELL.SAND,
        4: World.CELL.SAND_SHADOW,
        5: World.CELL.GROUND,
        6: World.CELL.GROUND_SHADOW
    };
    let map = JSON.parse(mapEncoded)
    map.n = parseInt(map.n);
    map.m = parseInt(map.m);
    for (let i = 0; i < map.n; i++) {
        for (let j = 0; j < map.m; j++) {
            map.data[i][j] = cellByNum[map.data[i][j]]
        }
    }
    return map
}

function findPath(map, start, finish) {
    let n = map.n;
    let m = map.m;
    map = map.data;

    let time = Array.from(Array(n), () => new Array(m).fill(-1));
    let how = Array.from(Array(n), () => new Array(m).fill(-1));

    let dirs = [[World.COMMAND.LEFT, 0, -1], [World.COMMAND.RIGHT, 0, 1], [World.COMMAND.UP, -1, 0], [World.COMMAND.DOWN, 1, 0]];

    let q = [{x: start.x, y: start.y}];
    time[start.x][start.y] = 0;

    let canMove = function(cell) {
        return 0 <= cell.x && cell.x < n && 0 <= cell.y && 0 <= cell.y && cell.y < m && map[cell.x][cell.y].passable;
    };

    while (q.length > 0) {
        let current = q.shift();
        dirs.forEach((dir) => {
                let nxt = {x: current.x + dir[1], y: current.y + dir[2]};
                let ntime = time[current.x][current.y] + 1;
                if (canMove(nxt) && time[nxt.x][nxt.y] === -1) {
                    time[nxt.x][nxt.y] = ntime;
                    how[nxt.x][nxt.y] = dir;
                    q.push(nxt);
                }
            }
        );
    }

    let cmds0 = []

    if (how[finish.x][finish.y] === -1) {
        throw "No path!";
    }

    while (finish.x !== start.x || finish.y !== finish.x) {
        let dir = how[finish.x][finish.y];
        cmds0.push(dir[0]);
        finish.x -= dir[1];
        finish.y -= dir[2];
    }
    cmds0 = cmds0.reverse();

    let rover = new World.Rover(start.x, start.y, map);
    let cmds = {"cmds": []};
    cmds0.forEach(cmd => {
        while (!rover.canMove(cmd)) {
            rover.doCommand(World.COMMAND.CHARGE);
            cmds.cmds.push(World.COMMAND.CHARGE);
        }
        rover.doCommand(cmd);
        cmds.cmds.push(cmd);
    });

    return cmds;
}

app.get("/path", jsonParser, function (req, res) {
    if (!req.body) {
        res.status(404).send();
    }
    let id = req.body.id;
    let start = {x: parseInt(req.body.start.y), y: parseInt(req.body.start.y)};
    let finish = {x: parseInt(req.body.finish.y), y: parseInt(req.body.finish.y)};
    try {
        let map = fs.readFileSync(path.join(directoryPath, "map" + id + ".json")).toString();
        try {
            let commands = findPath(buildMapFromJSON(map), start, finish);
            res.send(commands);
        } catch (err) {
            res.status(400).send();
        }

    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        }
    }
})

app.listen(8080, function () {
    console.log("Server created!");
})