$(document).ready(function () {
    var sourceSwap = function () {
        var $this = $(this);
        var newSource = $this.data('alt-src');
        $this.data('alt-src', $this.attr('src'));
        $this.attr('src', newSource);
    }
    $('.imgDelete').hover(sourceSwap, sourceSwap);
    $('.removeLink').css('visibility', 'hidden');
    $('#TrimLinks span').hover(
            function () {
                $(this).find('.removeLink').css('visibility', 'visible');
            },
            function () {
                $(this).find('.removeLink').css('visibility', 'hidden');
            }
        );
    $('#TrimLinks span a').hover(
            function () {
                $(this).find('.removeLink').css('visibility', 'visible');
            },
            function () {
                $(this).find('.removeLink').css('visibility', 'hidden');
            }
        );
    $(".expando").hover(function () {
        $('ul', this).toggle();
    });
    $(".openDialog").on("click", function (e) {
        e.preventDefault();
        var title = $(this).data('title');
        $('#dialog').load(this.href, function () {
            $(this).dialog({
                title: title,
                modal: true,
                width: 600,
                resizable: false
            })
            bindForm(this);
        });
        return false;
    });

    function bindForm(dialog) {
        $('form', dialog).submit(function () {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    if (result.success) {
                        //redraw page
                        location.reload();
                    } else {
                        $('#dialog').html(result);
                        bindForm();
                    }
                }
            });
            return false;
        });
    }
});