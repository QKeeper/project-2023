copy_buttons = document.querySelectorAll("button.copy");

for (let button of copy_buttons) {
	button.addEventListener("click", copy_handler);
}

function copy_handler(e) {
	checkbox = e.target.parentNode.parentNode.querySelector("input");
	checkbox.checked = true;
	notify("ID скопирован в буфер обмена");

	id = e.target.parentNode.parentNode.querySelector(".id").textContent;
	navigator.clipboard.writeText(id);
}

timeouts = [];
function notify(text) {
	res = document.querySelectorAll(".notify");

	for (let el of res) {
		el.remove();
	}
	timeouts.map((t) => clearTimeout(t));

	document.body.insertAdjacentHTML(
		`beforeend`,
		`<div class="notify">
			<div class="content">
				<img src="../src/check.svg" />
				<p>${text}</p>
			</div>
			<div class="progress"></div>
		</div>`
	);
	el = document.querySelector(".notify");
	el.style.animationTimingFunction = "ease-in-out";
	height = el.getBoundingClientRect().height;

	el.animate([{ bottom: `-${height}px` }, { bottom: `24px` }], {
		duration: 300,
		iterations: 1,
		easing: "cubic-bezier(0, 0.55, 0.45, 1)",
	});

	progress = el.querySelector(".progress");
	progress.animate([{ width: "100%" }, { width: "0%" }], {
		duration: 3000,
		iterations: 1,
	});

	timeouts.push(
		setTimeout(() => {
			progress.style.width = "0";
		}, 2950)
	);

	timeouts.push(
		setTimeout(() => {
			el.animate([{ bottom: `24px` }, { bottom: `-${height}px` }], {
				duration: 300,
				iterations: 1,
				easing: "cubic-bezier(0.55, 0, 1, 0.45)",
			});

			timeouts.push(
				setTimeout(() => {
					el.remove();
				}, 250)
			);
		}, 3000)
	);
}
