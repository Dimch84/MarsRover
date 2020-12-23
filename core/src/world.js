

const Constants = Object.freeze({
    MAX_ENERGY: 100,

    MOVE_ENERGY: 3,
    HARD_MOVE_ENERGY: 6,

    CHARGE_SUN: 3,
    CHARGE_SHADOW: 1,
});


const COMMAND = Object.freeze({LEFT: "LEFT", RIGHT: "RIGHT", UP: "UP", DOWN: "DOWN", CHARGE: "CHARGE"})

class Cell {
    constructor(name, passable, moveCost, energyCharge) {
        this.name = name
        this.passable = passable
        this.moveCost = moveCost
        this.energyCharge = energyCharge
    }
}

const CELL = Object.freeze({
    "ROCK": new Cell("ROCK", false, -1, 0),
    "RAVINE": new Cell("RAVINE", false, -1, 0),
    "SAND": new Cell("SAND", true, Constants.HARD_MOVE_ENERGY, Constants.CHARGE_SUN),
    "SAND_SHADOW": new Cell("SAND_SHADOW", true, Constants.HARD_MOVE_ENERGY, Constants.CHARGE_SHADOW),
    "GROUND": new Cell("GROUND", true, Constants.MOVE_ENERGY, Constants.CHARGE_SUN),
    "GROUND_SHADOW": new Cell("GROUND_SHADOW", true, Constants.MOVE_ENERGY, Constants.CHARGE_SHADOW)
})



class Rover {
    constructor(x, y, map) {
        this.x = x;
        this.y = y;

        this.map = map;
        this.n = map.length;
        this.m = map[0].length;

        this.energy = Constants.MAX_ENERGY;
    }

    canMove(cmd) {
        let was_x = this.x;
        let was_y = this.y;

        switch (cmd) {
            case COMMAND.LEFT:
                was_y--;
                break;
            case COMMAND.RIGHT:
                was_y++;
                break;
            case COMMAND.UP:
                was_x--;
                break;
            case COMMAND.DOWN:
                was_x++;
                break;
            default:
                throw "Bad command!";
        }
        return (0 <= was_x && was_x < this.n && 0 <= was_y && was_y < this.m && this.energy >= this.map[was_x][was_y].moveCost);
    }

    doCommand(cmd) {
        let was_x = this.x;
        let was_y = this.y;
        let was_energy = this.energy;
        switch (cmd) {
            case COMMAND.LEFT:
                this.y--;
                this.energy -= this.map[this.x][this.y].moveCost;
                break;
            case COMMAND.RIGHT:
                this.y++;
                this.energy -= this.map[this.x][this.y].moveCost;
                break;
            case COMMAND.UP:
                this.x--;
                this.energy -= this.map[this.x][this.y].moveCost;
                break;
            case COMMAND.DOWN:
                this.x++;
                this.energy -= this.map[this.x][this.y].moveCost;
                break;
            case COMMAND.CHARGE:
                this.energy = Math.min(this.energy + this.map[this.x][this.y].energyCharge, Constants.MAX_ENERGY);
                break;
            default:
                throw "Bad command!";
        }
        if (this.energy < 0 || !this.map[this.x][this.y].passable) {
            this.x = was_x;
            this.y = was_y;
            this.energy = was_energy;
            throw (this.energy < 0 ? "Low energy!" : "Can't move!");
        }
    }
}

exports.Constants = Constants
exports.COMMAND = COMMAND
exports.CELL = CELL
exports.Rover = Rover
exports.Cell = Cell