// يمكنك إضافة أي تفاعلات JavaScript هنا
document.addEventListener('DOMContentLoaded', function () {
    // تأثيرات للبطاقات
    const filmCards = document.querySelectorAll('.film-card');

    filmCards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transform = 'translateY(-5px)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = '';
        });
    });

    // تأكيد عند تطبيق الفلتر
    document.querySelector('.filter-form').addEventListener('submit', function (e) {
        console.log('Applying filters...');
    });
});
// Custom JavaScript for CinemaPro Dashboard
document.addEventListener('DOMContentLoaded', function () {
    // Toggle sidebar
    document.getElementById('sidebarToggle').addEventListener('click', function (e) {
        e.preventDefault();
        document.body.classList.toggle('sb-sidenav-toggled');
        localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
    });

    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Search functionality
    const searchForm = document.querySelector('.sb-topnav form');
    if (searchForm) {
        searchForm.addEventListener('submit', function (e) {
            e.preventDefault();
            const searchTerm = this.querySelector('input').value.trim();
            if (searchTerm) {
                // Implement search functionality here
                console.log('Searching for:', searchTerm);
                // window.location.href = `/Search?query=${encodeURIComponent(searchTerm)}`;
            }
        });
    }

    // Dark mode preference
    const prefersDarkScheme = window.matchMedia('(prefers-color-scheme: dark)');
    if (prefersDarkScheme.matches) {
        document.body.classList.add('dark-mode');
    }
});

// Custom confirm dialogs
function showConfirmDialog(options) {
    return Swal.fire({
        title: options.title || 'Are you sure?',
        text: options.text || '',
        icon: options.icon || 'warning',
        showCancelButton: true,
        confirmButtonColor: options.confirmColor || '#d81f26',
        cancelButtonColor: options.cancelColor || '#6c757d',
        confirmButtonText: options.confirmText || 'Confirm',
        cancelButtonText: options.cancelText || 'Cancel'
    });
}