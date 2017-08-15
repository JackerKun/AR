var canvas;
var context;

// 初始化
window.onload = function () {
    // 获取画布已经绘图上下文
    canvas = document.getElementById("drawingCanvas");
    context = canvas.getContext("2d");
    context.fillStyle = 'rgba(255, 255, 255, 0)';

    // 画布添加鼠标事件
    canvas.onmousedown = startDrawing;
    canvas.onmouseup = stopDrawing;
    canvas.onmouseout = stopDrawing;
    canvas.onmousemove = drawPic;

    canvas.addEventListener('touchstart', touchStartDrawing, false);
    canvas.addEventListener('touchmove',touchDrawing,false);
    canvas.addEventListener('touchend', touchStopDrawing,false);
    canvas.height = window.innerHeight/2;
    canvas.width = window.innerWidth;
};

// 记录当前是否在画图
var isDrawing = false;

//touch
var lastX;
var lastY;
var currentX;
var currentY;

// 开始画图
function startDrawing(e) {
    isDrawing = true;
    // 创建一个新的绘图路径
    context.beginPath();
    // 把画笔移动到鼠标位置
    context.moveTo(e.pageX - canvas.offsetLeft, e.pageY - canvas.offsetTop);
}

// 停止画图
function stopDrawing() {
    isDrawing = false;
}

//画图中
function drawPic(e) {
    if (isDrawing == true) {
        // 找到鼠标最新位置
        var x = e.pageX - canvas.offsetLeft;
        var y = e.pageY - canvas.offsetTop;
        // 画一条直线到鼠标最新位置
        context.lineTo(x, y);
        context.stroke();
    }
}

function touchStartDrawing(event) {
    touch = event.touches[0];
    console.log(touch);

    if(touch.pageX!==undefined){
        lastX = touch.pageX;
        lastY = touch.pageY
    }

    // begins new line
    context.beginPath();

    isDrawing = true;
}

function touchDrawing(event) {
    if(isDrawing){
        // get current mouse position
        touch = event.touches[0];
        // console.log(touch);
        if(touch.pageX!==undefined){
            currentX = touch.pageX;
            currentY = touch.pageY
        }

        draw(lastX, lastY, currentX, currentY);

        // set current coordinates to last one
        lastX = currentX;
        lastY = currentY;
    }
}

function touchStopDrawing(event) {
    isDrawing = false;
}


// 清除画布
function clearCanvas() {
    context.clearRect(0, 0, canvas.width, canvas.height);
}

function draw(lX, lY, cX, cY){
    // line from
    context.moveTo(lX,lY);
    // to
    context.lineTo(cX,cY);
    // color
    context.strokeStyle = "#000";
    // draw it
    context.stroke();
}

// 保存画布
function saveCanvas() {
    // 找到预览的 <img> 元素标签
    var imageCopy = document.getElementById("savedImageCopy");
    // 将画布内容在img元素中显示
    imageCopy.src = canvas.toDataURL();

    function exportCanvasAsPNG(id, fileName) {

        var canvasElement = document.getElementById(id);

        var MIME_TYPE = "image/png";

        var imgURL = canvasElement.toDataURL(MIME_TYPE);

        var dlLink = document.createElement('a');
        dlLink.download = fileName;
        dlLink.href = imgURL;
        dlLink.dataset.downloadurl = [MIME_TYPE, dlLink.download, dlLink.href].join(':');

        document.body.appendChild(dlLink);
        dlLink.click();
        document.body.removeChild(dlLink);
    }

    exportCanvasAsPNG('drawingCanvas','test.png')
   
}
