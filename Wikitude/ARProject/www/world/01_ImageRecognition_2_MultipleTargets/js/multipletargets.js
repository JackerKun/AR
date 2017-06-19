var World = {
	loaded: false,

	init: function initFn() {
		this.createOverlays();
	},

	createOverlays: function createOverlaysFn() {
		/*
			First an AR.ImageTracker needs to be created in order to start the recognition engine. It is initialized with a AR.TargetCollectionResource specific to the target collection that should be used. Optional parameters are passed as object in the last argument. In this case a callback function for the onTargetsLoaded trigger is set. Once the tracker loaded all its target images, the function worldLoaded() is called.

			Important: If you replace the tracker file with your own, make sure to change the target name accordingly.
			Use a specific target name to respond only to a certain target or use a wildcard to respond to any or a certain group of targets.

			Adding multiple targets to a target collection is straightforward. Simply follow our Target Management Tool documentation. Each target in the target collection is identified by its target name. By using this target name, it is possible to create an AR.ImageTrackable for every target in the target collection.
		*/
		this.targetCollectionResource = new AR.TargetCollectionResource("assets/tracker.wtc", {
		});

		this.tracker = new AR.ImageTracker(this.targetCollectionResource, {
			// onTargetsLoaded: this.worldLoaded,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});

		/*
			The next step is to create the augmentation. In this example an image resource is created and passed to the AR.ImageDrawable. A drawable is a visual component that can be connected to an IR target (AR.ImageTrackable) or a geolocated object (AR.GeoObject). The AR.ImageDrawable is initialized by the image and its size. Optional parameters allow for position it relative to the recognized target.
		*/

		// Create overlay for page one
		var img1 = new AR.ImageResource("assets/b1.png");
		var overlay1 = new AR.ImageDrawable(img1, 1);
		var page1 = new AR.ImageTrackable(this.tracker, "b1", {
			drawables: {
				cam: overlay1
			},
			onImageRecognized: this.removeLoadingBar,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});

		/*
			Similar to the first part, the image resource and the AR.ImageDrawable for the second overlay are created.
		*/
		var img3 = new AR.ImageResource("assets/b3.png");
		var overlay3 = new AR.ImageDrawable(img3, 1);
		var page3 = new AR.ImageTrackable(this.tracker, "b3", {
			drawables: {
				cam: overlay3
			},
			onImageRecognized: this.removeLoadingBar,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});

		var img4 = new AR.ImageResource("assets/b4.png");
		var overlay4 = new AR.ImageDrawable(img4, 1);
		var page4 = new AR.ImageTrackable(this.tracker, "b4", {
			drawables: {
				cam: overlay4
			},
			onImageRecognized: this.removeLoadingBar,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});

		var img5 = new AR.ImageResource("assets/b5.png");
		var overlay5 = new AR.ImageDrawable(img5, 1, {
			rotate: {
				z: 90
			}
		});
		var page5 = new AR.ImageTrackable(this.tracker, "b5", {
			drawables: {
				cam: overlay5
			},
			onImageRecognized: this.removeLoadingBar,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});


		var img6 = new AR.ImageResource("assets/b6.png");
		var overlay6 = new AR.ImageDrawable(img6, 1, {
			rotate: {
				z: 90
			}
		});
		var page6 = new AR.ImageTrackable(this.tracker, "b6", {
			drawables: {
				cam: overlay6
			},
			onImageRecognized: this.removeLoadingBar,
            onError: function(errorMessage) {
            	alert(errorMessage);
            }
		});
	},

	removeLoadingBar: function() {
		if (!World.loaded) {
			var e = document.getElementById('loadingMessage');
			e.parentElement.removeChild(e);
			World.loaded = true;
		}
	},

	worldLoaded: function worldLoadedFn() {
		var cssDivInstructions = " style='display: table-cell;vertical-align: middle; text-align: right; width: 50%; padding-right: 15px;'";
		var cssDivSurfer = " style='display: table-cell;vertical-align: middle; text-align: left; padding-right: 15px; width: 38px'";
		var cssDivBiker = " style='display: table-cell;vertical-align: middle; text-align: left; padding-right: 15px;'";
		document.getElementById('loadingMessage').innerHTML =
			"<div" + cssDivInstructions + ">Scan Target &#35;1 (surfer) or &#35;2 (biker):</div>" +
			"<div" + cssDivSurfer + "><img src='assets/surfer.png'></img></div>" +
			"<div" + cssDivBiker + "><img src='assets/bike.png'></img></div>";
	}
};

World.init();
