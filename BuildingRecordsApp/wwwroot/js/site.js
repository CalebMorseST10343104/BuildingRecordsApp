// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.querySelectorAll('[data-filtered-index]').forEach(index => {
    const filters = [...index.querySelectorAll('[data-column-filter]')];
    const rows = [...index.querySelectorAll('[data-record-row]')];
    const emptyRow = index.querySelector('[data-no-filter-results]');

    const applyFilters = () => {
        let visibleCount = 0;
        rows.forEach(row => {
            const matches = filters.every(filter => {
                const term = filter.value.trim().toLocaleLowerCase();
                if (!term) return true;
                const cell = row.children[Number(filter.dataset.columnFilter)];
                return (cell?.dataset.filterValue ?? cell?.textContent ?? '').toLocaleLowerCase().includes(term);
            });
            row.hidden = !matches;
            const actions = index.querySelector(`[data-record-actions="${row.dataset.recordRow}"]`);
            if (actions) actions.hidden = !matches;
            if (matches) visibleCount++;
        });
        if (emptyRow) emptyRow.hidden = visibleCount !== 0;
    };

    filters.forEach(filter => filter.addEventListener('input', applyFilters));
    applyFilters();
});

document.querySelectorAll('[data-card-filter]').forEach(filter => {
    const cards = [...document.querySelectorAll(filter.dataset.cardFilter)];
    const empty = document.querySelector('[data-card-filter-empty]');
    const applyFilter = () => {
        const term = filter.value.trim().toLocaleLowerCase();
        let visibleCount = 0;
        cards.forEach(card => {
            const matches = !term || (card.dataset.filterValue ?? card.textContent ?? '').toLocaleLowerCase().includes(term);
            card.hidden = !matches;
            if (matches) visibleCount++;
        });
        if (empty) empty.hidden = visibleCount !== 0;
    };
    filter.addEventListener('input', applyFilter);
    applyFilter();
});

document.querySelectorAll('[data-export-scope-form]').forEach(form => {
    const property = form.querySelector('[data-export-property]');
    const buildingOptions = [...form.querySelectorAll('[data-property-id]')];
    const updateBuildings = () => {
        const selectedProperty = property?.value ?? '';
        buildingOptions.forEach(option => {
            const visible = selectedProperty !== '' && option.dataset.propertyId === selectedProperty;
            option.hidden = !visible;
            if (!visible) {
                const checkbox = option.querySelector('input[type="checkbox"]');
                if (checkbox) checkbox.checked = false;
            }
        });
    };
    property?.addEventListener('change', updateBuildings);
    updateBuildings();
});
