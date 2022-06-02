var messageBuilder = function () {
    var message = null;
    var p = null;
    var footer = null;

    return {
        createMessage: function (classList) {
            message = document.createElement("div")
            if (classList === undefined)
                classList = [];

            for (var i = 0; i < classList.length; i++) {
                message.classList.add(classList[i])
            }

            message.classList.add('message')
            return this;
        },
        withParagraph: function (text1,text2) {
            p = document.createElement("div")
            p.classList.add("message-content");
            p.appendChild(document.createTextNode(text1 + ": " + text2))
            return this;
        },
        withFooter: function (text) {
            footer = document.createElement("div")
            footer.classList.add("message-time");
            footer.appendChild(document.createTextNode(text))
            return this;
        },
        build: function () {
            message.appendChild(p);
            message.appendChild(footer);
            return message;
        }
    }
}