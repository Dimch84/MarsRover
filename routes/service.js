var express = require('express');
var router = express.Router();

const bodyParser = require('body-parser');
const path = require('path');
const fs = require('fs');

const directoryPath = path.join("..", "resources");
const mapsFileName = "maps-info.json";


const World = require('./world')

var jsonParser = bodyParser.json();

process.chdir(__dirname);

maps = JSON.parse(fs.readFileSync(path.join(directoryPath, mapsFileName)));

//console.log(__dirname);

//
router.get('/health', function(req, res, next) {
  res.send('Service is working!');
});

//

router.get("/map", function (req, res) {
    res.send(maps);
})

//
router.get("/map/:id", function (req, res) {
    var id = req.params.id;
    try {
        var map = fs.readFileSync(path.join(directoryPath, "map" + id + ".json")).toString();
        res.send(map);
    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        }
    }
})

////////////////////////

function buildMapFromJSON(map) {
    const cellByNum = {
        1: World.CELL.ROCK,
        2: World.CELL.RAVINE,
        3: World.CELL.SAND,
        4: World.CELL.SAND_SHADOW,
        5: World.CELL.GROUND,
        6: World.CELL.GROUND_SHADOW
    };
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

    while (finish.x !== start.x || finish.y !== start.y) {
        let dir = how[finish.x][finish.y];
        cmds0.push(dir[0]);
        finish.x -= dir[1];
        finish.y -= dir[2];
    }
    cmds0 = cmds0.reverse();

    let rover = new World.Rover(start.x, start.y, map);
    let path = {"cmds": []};
    cmds0.forEach(cmd => {
        while (!rover.canMove(cmd)) {
            rover.doCommand(World.COMMAND.CHARGE);
            path.cmds.push(World.COMMAND.CHARGE);
        }
        rover.doCommand(cmd);
        path.cmds.push(cmd);
    });
    path.cost = path.cmds.length;
    path.energy = rover.energy;

    return path;
}

router.get("/path", jsonParser, function (req, res) {
    if (!req.body) {
        res.status(404).send();
    }
    let id = req.body.id;
    let start = {x: parseInt(req.body.start.x), y: parseInt(req.body.start.y)};
    let finish = {x: parseInt(req.body.finish.x), y: parseInt(req.body.finish.y)};
    try {
        let map = fs.readFileSync(path.join(directoryPath, "map" + id + ".json")).toString();
        try {
            let path = findPath(buildMapFromJSON(JSON.parse(map)), start, finish);
            res.send(path);
        } catch (err) {
            res.status(400).send();
        }

    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        }
    }
});

router.get("/path/map", jsonParser, function (req, res) {
    if (!req.body) {
        res.status(404).send();
    }
    try {
        let start = {x: parseInt(req.body.start.x), y: parseInt(req.body.start.y)};
        let finish = {x: parseInt(req.body.finish.x), y: parseInt(req.body.finish.y)};
        let map = req.body.map;
        try {
            let path = findPath(buildMapFromJSON(map), start, finish);
            res.send(path);
        } catch (err) {
            res.status(400).send();
        }

    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        }
    }
});

function getPathCost(map, cmds, start, finish) {
    map = map.data;

    let rover = new World.Rover(start.x, start.y, map);
    cmds.forEach(cmd => {
        if (!rover.canMove(cmd)) {
            throw "Bad commands! Rover can't move";
        }
        rover.doCommand(cmd);
    });
    if (rover.x !== finish.x || rover.y !== finish.y) {
        throw "Bad commands! Rover has not reached the finish";
    }
    return {"cost": cmds.length, "energy": rover.energy};
}

router.get("/path/cost", jsonParser, function (req, res) {
    if (!req.body) {
        res.status(404).send();
    }
    try {
        let id = req.body.id;
        let start = {x: parseInt(req.body.start.x), y: parseInt(req.body.start.y)};
        let finish = {x: parseInt(req.body.finish.x), y: parseInt(req.body.finish.y)};
        let pth = req.body.path;
        let map = fs.readFileSync(path.join(directoryPath, "map" + id + ".json")).toString();
        try {
            let path = getPathCost(buildMapFromJSON(JSON.parse(map)), pth, start, finish);
            res.send(path);
        } catch (err) {
            res.status(400).send(err);
        }

    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        } else {
            res.status(404).send(err);
        }
    }
});

router.get("/path/cost_map", jsonParser, (req, res) => mapCostReqProc(req, res)
);

router.post("/path/cost_map", jsonParser, (req, res) => mapCostReqProc(req, res)  
);

function mapCostReqProc(req, res) {
    if (!req.body) {
        res.status(404).send();
    }
    try {
        let start = {x: parseInt(req.body.start.x), y: parseInt(req.body.start.y)};
        let finish = {x: parseInt(req.body.finish.x), y: parseInt(req.body.finish.y)};
        let pth = req.body.path;
        let map = req.body.map; // map is already JSON object here
        try {
            let path = getPathCost(buildMapFromJSON(map), pth, start, finish);
            res.send(path);
        } catch (err) {
            res.status(400).send(err);
        }

    } catch (err) {
        if (err.code === "ENOENT") {
            res.status(404).send();
        } else {
            res.status(404).send(err);
        }
    }
}

////////////////////////
module.exports = router;
