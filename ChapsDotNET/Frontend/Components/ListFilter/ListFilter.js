import './ListFilter.scss';

export default function () {
    const filterWrapper = document.querySelector('.moj-filter-layout')

    if (!filterWrapper) {
        return;
    }

    const filterPanel = filterWrapper.querySelector('.moj-filter')
    const button = filterWrapper.querySelector('.moj-action-bar__filter input');

    button.addEventListener('click', (e) => {
        e.preventDefault();
        filterPanel.classList.toggle('moj-js-hidden');
        button.value = button.value === 'Close filter' ? 'Open filter' : 'Close filter';
        button.setAttribute('aria-expanded', button.getAttribute('aria-expanded') === 'true' ? 'false' : 'true');
    });

    // TODO: Temporary fix due to time constraints, need to properly persist form values
    // This intercepts clicks on the pagination links when in a list-filter and instead 
    // 1. updates a hidden page field on the form filter
    // 2. submits the form
    // This ensures that form values persist when the page is changed.
    const pagination = filterWrapper.querySelector('.chaps-pagination');
    const hiddenField = filterWrapper.querySelector('#list-filter-page');
    const form = filterWrapper.querySelector('form');
    if (pagination && hiddenField) {
        const links = pagination.querySelectorAll('.govuk-pagination__link');
        links.forEach((link) => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                const url = new URL(e.target.href);
                const params = new URLSearchParams(url.search);
                const page = params.get('page') || 1;
                hiddenField.value = page;
                form.submit();
            });
        });
    } 
}
