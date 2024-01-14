window.YaAuthSuggest.init(
	{
		client_id: "3d1521abaf6f431d95945effad470a80",
		response_type: "token",
		redirect_uri: "https://qkeeper.github.io/token",
	},
	"https://qkeeper.github.io/",
	{
		view: "button",
		parentId: "buttonContainerId",
		buttonSize: "m",
		buttonView: "main",
		buttonTheme: "light",
		buttonBorderRadius: "0",
		buttonIcon: "ya",
	}
)
	.then(({ handler }) => handler())
	.then((data) => console.log("Сообщение с токеном", data))
	.catch((error) => console.log("Обработка ошибки", error));
