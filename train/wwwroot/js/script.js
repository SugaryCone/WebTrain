const myAudio = document.createElement("audio");
const clickAudio = document.createElement("audio");
const radioAudio = document.createElement("audio");
var IsWork = false;
var IsWorkRadio = false;

let created = false;
let radiocreated = false;



var volume = 0.5;
var radiovolume = 0.5;

clickAudio.volume = 0.1;
myAudio.volume = volume;
radioAudio.volume = radiovolume;

var fm_slider = $("#train");
var radio_slider = $("#stream");


console.log(volume * 180)
fm_slider.css('transform', 'rotate(' + (volume * 180) + 'deg)');
radio_slider.css('transform', 'rotate(' + (radiovolume * 180) + 'deg)');

$(document).ready(function () {
	if (clickAudio.canPlayType("audio/mpeg")) {
		clickAudio.setAttribute("src", "../train/Click.mp3");
	}


	if (myAudio.canPlayType("audio/mpeg")) {
		myAudio.setAttribute("src", "https://concentrain.ru/icecast/stream");
	}

	if (radioAudio.canPlayType("audio/mpeg")) {
		radioAudio.setAttribute("src", "https://concentrain.ru/icecast/stream2");
		radioAudio.load();
	}
	

	set_clock();
	UpdateWay()
	var text = "Нажмите на красную кнопку";

	$('.display').html("<span id='displaytext' data-track='" + text + "'>" + text + "</span>");





	$("#switch1").change(function (e) {
		clickAudio.play();
		if (radioAudio.paused) {
			console.log("play");
			radioAudio.play();
		} else {
			radioAudio.pause();
			
		}

/*		if (!radiocreated) {
			
			radiocreated = true;
			
		}
		else {
			radioAudio.pause();
			radiocreated = false;
			console.log("noplay");
		}*/


	})

});


let WayDuration = 0;
let isLoadBack = false;
let elapsed_seconds = 0;



Draggable.create(fm_slider, {
	type: "rotation",
	throwProps: true,
	bounds: {
		minRotation: 0,
		maxRotation: 200,
	},
	liveSnap: function (endValue) {

		console.log(endValue / 200);
		volume = endValue / 200;
		myAudio.volume = volume;

		return Math.round(endValue / 20) * 20;
	}
});

Draggable.create(radio_slider, {
	type: "rotation",
	throwProps: true,
	bounds: {
		minRotation: 0,
		maxRotation: 200,
	},
	liveSnap: function (endValue) {

		console.log(endValue / 200);
		radiovolume = endValue / 200;
		radioAudio.volume = radiovolume;

		return Math.round(endValue / 20) * 20;
	}
});


$("#play").click(function (e) {
	clickAudio.play();
	if ($(e.currentTarget).hasClass("on")) {

		$(e.currentTarget).removeClass("on")

		created = false;
		$("button").toggle();

		myAudio.pause();

	} else {

		$(e.currentTarget).addClass("on")

		if (!created) {
			myAudio.play();
			setInterval(set_clock, 15000);
			$("#helloline").hide();
			created = true;
		}
	}
})


function set_clock() {
	var d = new Date();
	var s = d.getSeconds();
	var m = d.getMinutes();
	var h = d.getHours();
	var line = ("0" + h).substr(-2) + ":" + ("0" + m).substr(-2) + ":" + ("0" + s).substr(-2);
	$('.display').html("<span id='displaytext' data-track='" + line + "'>" + line + "</span>");

}





function UpdateWay() {
	$.getJSON("https://localhost:7191/preload", function (data) {
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
	$.getJSON("https://localhost:7191/api/way", function (data) {

		var dateStr = JSON.parse('"' + data["departureTime"]+ '"');
		console.log(dateStr); // 2014-01-01T23:28:56.782Z

		var date = new Date(dateStr);
		console.log(date);
		console.log(Date.now() - date); 
		var server_gap = Date.now() - date;
		$(data["backgrounds"]).each(function (id, back) {
			
			var d_sample_time = 30 * data["backgroundsSampleCount"][id] * 1000;
			if (d_time >= server_gap) {
				console.log(d_time - server_gap);

				setTimeout(SetBack, d_time - server_gap, "https://localhost:7191/" + back["fileName"]);
				console.log("https://localhost:7191/" + back["fileName"]);
			}
			else if (d_time + d_sample_time >= server_gap) {
				$("#background > source").attr("src", "https://localhost:7191/" + back["fileName"]); 
				
				document.getElementById("background").load();
				document.getElementById("background").currentTime = server_gap - d_time;
				document.getElementById("background").play();
				console.log("НЕПОЛНЫЙ");
				console.log(document.getElementById("background").currentTime);
			}
			else {
				/*server_gap -= d_sample_time;*/
			}
			
			d_time += d_sample_time;

		})
		setTimeout(UpdateWay, d_time + 10 - server_gap);
	});
}


function RunWayBacground(imgs, counts) {
	$("#background").src = imgs[0];
	console.log("VIDEO");
}





/*$(document).on("click", () => {
	if (!created) {
		myAudio.play();
		$("#helloline").hide();
		created = true;
		$("button").toggle();
	}
	else {

	}
});
*/


var start = new Date;


function get_elapsed_time_string(total_seconds) {
	function pretty_time_string(num) {
		return (num < 10 ? "0" : "") + num;
	}

	var hours = Math.floor(total_seconds / 3600);
	total_seconds = total_seconds % 3600;

	var minutes = Math.floor(total_seconds / 60);
	total_seconds = total_seconds % 60;

	var seconds = Math.floor(total_seconds);

	// Pad the minutes and seconds with leading zeros, if required
	hours = pretty_time_string(hours);
	minutes = pretty_time_string(minutes);
	seconds = pretty_time_string(seconds);

	// Compose the string for display
	var currentTimeString = hours + ":" + minutes + ":" + seconds;

	return currentTimeString;
}

