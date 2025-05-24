import './Form.scss';

export default function () {
    const form = document.querySelector(".chaps-form");
  
    if (!form) {
        return;
    }

    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
                const el = mutation.target;
                const group = el.closest('.govuk-form-group');
                const input = group.querySelector('input, select')
                if (!group && !input) {
                    return;
                }
                if (el.classList.contains('field-validation-error')) {
                    group.classList.add("govuk-form-group--error");
                    input.classList.add("govuk-input--error");
                }
                else if (el.classList.contains('field-validation-valid')) {
                    group.classList.remove("govuk-form-group--error");
                    input.classList.remove("govuk-input--error");
                }
            }
        });
    });

    const formGroups = form.querySelectorAll(".govuk-form-group");

    formGroups.forEach((group) => {
        const errorPlaceholders = group.querySelectorAll(".field-validation-valid");
        if (errorPlaceholders) {
            errorPlaceholders.forEach((placeholder) => {
                observer.observe(placeholder, {
                    attributes: true,
                    attributeFilter: ['class']
                });
            })
        }
    });
}
