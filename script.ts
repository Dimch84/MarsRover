const canvas: HTMLCanvasElement = <HTMLCanvasElement>document.getElementById("canvas")
let dataWidth;
let dataHeight;
let squareSize;
let ctx;
let canvasWidth;
let canvasHeight;
let curArrData;


async function saveMap() {
    const field: HTMLTextAreaElement = <HTMLTextAreaElement>document.getElementById("map_area")

    const arrData = await JSON.parse(field.value)

    for (let line of arrData) {
        console.log(line)
        if (line.length != arrData[0].length) {
            alert("Matrix is incorrect")
            return
        }
    }
    applyDataToMap(arrData)
}


function applyDataToMap(arrData: Array<String>[]) {
    canvas.hidden = false
    curArrData = arrData

    dataWidth = arrData[0].length
    dataHeight = arrData.length

    squareSize = Math.min(innerWidth * 0.95 / dataWidth, innerHeight * 0.95 / dataHeight)
    ctx = canvas.getContext('2d')
    canvasWidth = dataWidth * squareSize
    canvasHeight = dataHeight * squareSize

    canvas.setAttribute("width", String(dataWidth * squareSize))
    canvas.setAttribute("height", String(dataHeight * squareSize))

    console.log(dataHeight);
    console.log(dataWidth)

    refreshCanvas();


    (<HTMLButtonElement>document.getElementById("run_btn")).disabled = false
}

const patterns: Array<HTMLImageElement> = new Array<HTMLImageElement>(6);

function refreshCanvas(): void {
    ctx.fillStyle = "#ffe4ff";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    ctx.fillStyle = "#000000";
    ctx.font = Math.max(30, squareSize / 100) + "px Arial";

    for (let i = 0; i < dataHeight; i++) {
        for (let j = 0; j < dataWidth; j++) {
            ctx.fillText(curArrData[i][j], (j + 0.5) * squareSize, (i + 0.5) * squareSize, squareSize)
            ctx.drawImage(patterns[curArrData[i][j]], j * squareSize, i * squareSize, squareSize, squareSize)
        }
    }

}

function initCanvas(): void {
    const size = window.innerHeight
    canvas.setAttribute("width", String(size * 0.9))
    canvas.setAttribute("height", String(size * 0.95))
}

async function runWay() {
    refreshCanvas()

    const pathField = <HTMLTextAreaElement>document.getElementById("path_area")
    const pathObject = await JSON.parse(pathField.value)
    console.log(pathObject)

    const data = pathObject.path;
    const ctx = canvas.getContext('2d')
    ctx.beginPath();
    ctx.moveTo(squareSize * (pathObject.start.x - 0.5), squareSize * (pathObject.start.y - 0.5))

    var curX = squareSize / 2
    var curY = squareSize / 2

    const deltaX = {
        'UP': 0,
        'DOWN': 0,
        'RIGHT': +1,
        'LEFT': -1
    };

    const deltaY = {
        'UP': -1,
        'DOWN': +1,
        'RIGHT': 0,
        'LEFT': 0
    };
    for (const c of data) {
        curX += deltaX[c] * squareSize
        curY += deltaY[c] * squareSize
        ctx.lineTo(curX, curY)
        if (curX > canvasWidth || curY > canvasHeight || curX < 0 || curY < 0) {
            alert("You've lost")
            break;
        }
    }
    const savedWidth = ctx.lineWidth;
    ctx.lineWidth = 10;
    ctx.strokeStyle = "white"

    ctx.arc(curX, curY, squareSize / 6, 0, 2 * Math.PI);
    ctx.stroke();

    ctx.beginPath()
    ctx.arc(curX, curY, squareSize / 6, 0, 2 * Math.PI);
    ctx.fill();
    ctx.lineWidth = savedWidth;
    ctx.stroke()
}

function loadPictures() {
    for (let i = 0; i < 6; i++) {
        patterns[i] = new Image();
        patterns[i].src = "patterns/type" + (i + 1).toString() + ".png";
    }
}

function initView(): void {
    initCanvas();
    loadPictures();
}