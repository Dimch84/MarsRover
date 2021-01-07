const canvas: HTMLCanvasElement = <HTMLCanvasElement>document.getElementById("canvas")
let dataWidth;
let dataHeight;
let size;
let ctx;
let canvasWidth;
let canvasHeight;
let curArrData;


function saveMap(): void {
    const field: HTMLTextAreaElement = <HTMLTextAreaElement>document.getElementById("map_area")

    const str: String = String(field.value)
    if (str.match(/[^\d\n\s]/gi) != null) {
        alert("Во вводе не должно быть ничего кроме цифр")
        return
    }

    const arrData = str.split("\n").map(function (elem: String): Array<String> {
        return elem.split(" ")
    })
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
    curArrData = arrData

    dataWidth = arrData[0].length
    dataHeight = arrData.length

    size = Math.min(innerWidth * 0.95 / dataWidth, innerHeight * 0.95 / dataHeight)
    ctx = canvas.getContext('2d')
    canvasWidth = dataWidth * size
    canvasHeight = dataHeight * size

    canvas.setAttribute("width", String(dataWidth * size))
    canvas.setAttribute("height", String(dataHeight * size))

    console.log(dataHeight);
    console.log(dataWidth)

    clearCanvas();


    (<HTMLButtonElement>document.getElementById("run_btn")).disabled = false
}

function clearCanvas(): void {
    ctx.fillStyle = "#ffe4ff";
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    ctx.fillStyle = "#000000";
    ctx.font = Math.max(30, size / 100) + "px Arial";
    for (let i = 0; i < dataHeight; i++) {
        for (let j = 0; j < dataWidth; j++) {
            ctx.fillText(curArrData[i][j], (j + 0.5) * size, (i + 0.5) * size, size)
        }
    }
    for (let i = 0; i < dataWidth; i++) {
        ctx.beginPath();
        ctx.moveTo(i * size, canvasHeight);
        ctx.lineTo(i * size, 0);
        ctx.strokeStyle = "red";
        ctx.stroke();
    }

    for (let i = 0; i < dataHeight; i++) {
        ctx.beginPath();
        ctx.moveTo(canvasWidth, i * size);
        ctx.lineTo(0, i * size);
        ctx.strokeStyle = "red";
        ctx.stroke();
    }
}

function initCanvas(): void {
    const size = window.innerHeight
    canvas.setAttribute("width", String(size * 0.9))
    canvas.setAttribute("height", String(size * 0.95))

    const ctx = canvas.getContext('2d');

    const grad = ctx.createLinearGradient(0, 0, canvas.width, canvas.height);
    grad.addColorStop(0, "magenta");
    grad.addColorStop(0.5, "blue");
    grad.addColorStop(1.0, "red");

    ctx.fillStyle = grad;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    ctx.fillStyle = null
    ctx.fillText("Big smile!", 10, 90);
}

function runWay(): void {
    clearCanvas()

    const pathField = <HTMLTextAreaElement>document.getElementById("path_area")
    const data = pathField.value

    const ctx = canvas.getContext('2d')
    ctx.beginPath();
    ctx.moveTo(size / 2, size / 2)

    var curX = size / 2
    var curY = size / 2

    const deltaX = {
        'U': 0,
        'D': 0,
        'R': +1,
        'L': -1
    };

    const deltaY = {
        'U': -1,
        'D': +1,
        'R': 0,
        'L': 0
    };
    for (const c of data) {
        curX += deltaX[c] * size
        curY += deltaY[c] * size
        ctx.lineTo(curX, curY)
        if (curX > canvasWidth || curY > canvasHeight || curX < 0 || curY < 0) {
            alert("You've lost")
            break;
        }
    }
    var a = ctx.lineWidth
    ctx.lineWidth = 10;
    ctx.strokeStyle = "black"

    ctx.arc(curX, curY, size / 6, 0, 2 * Math.PI);
    ctx.stroke();

    ctx.beginPath()
    ctx.arc(curX, curY, size / 6, 0, 2 * Math.PI);
    ctx.fill();
    ctx.lineWidth = a;
    ctx.stroke()
}

function initView(): void {
    initCanvas()
}