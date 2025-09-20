// Sidebar Toggle Function
function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const backdrop = document.getElementById('sidebarBackdrop');

    sidebar.classList.toggle('show');
    backdrop.classList.toggle('show');
}

// Fonksiyonu global olarak erişilebilir yapın
window.toggleSidebar = toggleSidebar;