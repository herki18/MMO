$(document).ready(function() {
    $("a.post").click(function(e) {
        e.preventDefault();

        var $this = $(this);

        var message = $this.data("message");

        if (message && !confirm(message)) {
            return;
        }

        $("<form>")
            .attr("method", "post")
            .attr("action", $this.attr("href"))
            .appendTo(document.body)
            .submit();
    });
});