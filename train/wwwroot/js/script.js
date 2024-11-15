const myAudio = document.createElement("audio");

let WayDuration = 0;
let isLoadBack = false;



$(document).ready(function () {
	if (myAudio.canPlayType("audio/mpeg")) {
		myAudio.setAttribute("src", "http://89.22.225.13:8000/stream");
	}
	UpdateWay()

});

function UpdateWay() {
	$.getJSON("/preload", function (data) {
		//console.log(data);

		var l = { "files": data };
		console.log(l);

		$.html5Loader({
			filesToLoad: l,
			stopExecution: true,
			onElementLoaded: function (obj, elm) {
				console.log(obj);
				console.log(elm);
			},
			onComplete: function () {
				console.log("start window");
				GetWay();
			}


		});
	});
}

function SetBack(path) {
	$("#background > source").attr("src", path);
	document.getElementById("background").load();
}
//"https://localhost:7191/" + data["backLoad"][0]["fileName"]
function GetWay() {
	let d_time = 0;
	$.getJSON("/api/way", function (data) {
		$(data["backgrounds"]).each(function (id, back) {
			console.log(d_time);
			console.log("https://localhost:7191/" + back["fileName"]);
			setTimeout(SetBack, d_time, back["fileName"]);
			d_time += 30 * data["backgroundsSampleCount"][id] * 1000;

		})
		setTimeout(UpdateWay, d_time + 10);
	});
}


function RunWayBacground(imgs, counts) {
	$("#background").src = imgs[0];
	console.log("VIDEO");
}




let created = false;
$(document).on("click", () => {
	if (!created) {
		myAudio.play();
		created = true;
	}
	else {
		myAudio.pause();
		created = false;
	}
});