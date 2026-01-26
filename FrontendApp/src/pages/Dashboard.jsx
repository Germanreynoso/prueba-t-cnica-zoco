import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import api from '../services/api';
import { useNavigate } from 'react-router-dom';
import Modal from '../components/Modal';

const Dashboard = () => {
    const { user, logout } = useAuth();
    const { theme, toggleTheme } = useTheme();
    const [users, setUsers] = useState([]);
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [modalType, setModalType] = useState(''); // 'study', 'address', 'user'
    const [selectedItem, setSelectedItem] = useState(null);
    const navigate = useNavigate();

    // Trigger refresh for AdminView
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    useEffect(() => {
        if (user && user.role !== 'Admin') {
            fetchProfile();
        } else {
            setLoading(false);
        }
    }, [user]);

    const fetchProfile = async () => {
        try {
            const response = await api.get(`/users/${user.id}`);
            setProfile(response.data);
        } catch (error) {
            console.error("Error fetching profile", error);
        } finally {
            setLoading(false);
        }
    };

    const handleRefresh = () => {
        if (user?.role === 'Admin') {
            setRefreshTrigger(prev => prev + 1);
        } else {
            fetchProfile();
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const openModal = (type, item = null) => {
        setModalType(type);
        setSelectedItem(item);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedItem(null);
    };

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-950">
                <div className="flex flex-col items-center gap-4">
                    <div className="w-8 h-8 border-2 border-accent-500 border-t-transparent rounded-full animate-spin" />
                    <span className="text-gray-400 text-sm">Cargando...</span>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen transition-colors duration-300">
            {/* Navigation */}
            <nav className="sticky top-0 z-40 bg-[var(--bg-card)] backdrop-blur-xl border-b border-[var(--border-card)]">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex items-center justify-between h-16">
                        {/* Logo/Brand */}
                        <div className="flex items-center gap-3">
                            <img src="/logo.png" alt="Zoco Logo" className="w-8 h-8 object-contain" />
                            <span className="text-lg font-bold tracking-tight hidden sm:block">
                                ZOCO
                            </span>
                        </div>

                        {/* User info & Logout */}
                        <div className="flex items-center gap-3">
                            <button
                                onClick={toggleTheme}
                                className="p-2 rounded-lg text-[var(--text-muted)] hover:text-[var(--text-body)] hover:bg-[var(--bg-hover)] transition-colors"
                                title={theme === 'dark' ? 'Cambiar a modo claro' : 'Cambiar a modo oscuro'}
                            >
                                {theme === 'dark' ? (
                                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
                                    </svg>
                                ) : (
                                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" />
                                    </svg>
                                )}
                            </button>
                            <div className="hidden sm:flex items-center gap-2 px-3 py-1.5 rounded-lg bg-[var(--bg-secondary)] border border-[var(--border-secondary)]">
                                <div className="w-2 h-2 rounded-full bg-success-500" />
                                <span className="text-sm text-[var(--text-muted)]">
                                    <span className="font-medium text-[var(--text-body)]">{user?.username}</span>
                                    <span className="mx-1.5">•</span>
                                    <span>{user?.role}</span>
                                </span>
                            </div>
                            <button
                                onClick={handleLogout}
                                className="btn-ghost text-gray-400 hover:text-white px-3"
                                title="Cerrar sesión"
                            >
                                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
                                </svg>
                                <span className="hidden sm:inline ml-2">Salir</span>
                            </button>
                        </div>
                    </div>
                </div>
            </nav>

            {/* Main Content */}
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                {user?.role === 'Admin' ? (
                    <AdminView refreshTrigger={refreshTrigger} openModal={openModal} />
                ) : (
                    <UserView profile={profile} refresh={fetchProfile} openModal={openModal} />
                )}
            </main>

            {/* Modal */}
            <Modal
                isOpen={isModalOpen}
                onClose={closeModal}
                title={selectedItem ? `Editar ${modalType}` : `Agregar ${modalType}`}
            >
                {modalType === 'study' && <StudyForm item={selectedItem} userId={user?.id} onSuccess={() => { closeModal(); handleRefresh(); }} />}
                {modalType === 'address' && <AddressForm item={selectedItem} userId={user?.id} onSuccess={() => { closeModal(); handleRefresh(); }} />}
                {modalType === 'user' && <UserForm item={selectedItem} onSuccess={() => { closeModal(); handleRefresh(); }} />}
            </Modal>
        </div>
    );
};

// ============================================
// Admin View
// ============================================
const AdminView = ({ refreshTrigger, openModal }) => {
    const [users, setUsers] = useState([]);
    const [sessionLogs, setSessionLogs] = useState([]);
    const [view, setView] = useState('users'); // 'users' or 'logs'

    // Pagination & Search State
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const [searchTerm, setSearchTerm] = useState('');
    const [debouncedSearch, setDebouncedSearch] = useState('');
    const [isLoadingUsers, setIsLoadingUsers] = useState(false);

    // Debounce search
    useEffect(() => {
        const timer = setTimeout(() => {
            setDebouncedSearch(searchTerm);
            setPage(1); // Reset to page 1 on search
        }, 300);
        return () => clearTimeout(timer);
    }, [searchTerm]);

    useEffect(() => {
        if (view === 'users') {
            fetchUsers();
        } else {
            fetchLogs();
        }
    }, [view, page, debouncedSearch, refreshTrigger]);

    const fetchUsers = async () => {
        setIsLoadingUsers(true);
        try {
            const response = await api.get('/users', {
                params: {
                    page,
                    pageSize: 9, // 9 items grid
                    search: debouncedSearch
                }
            });
            // Handle both paginated and non-paginated responses for backward compatibility if needed
            if (response.data.items) {
                setUsers(response.data.items);
                setTotalPages(response.data.totalPages);
                setTotalCount(response.data.totalCount);
            } else {
                // Fallback if backend not updated yet
                setUsers(response.data);
            }
        } catch (error) {
            console.error("Error fetching users", error);
        } finally {
            setIsLoadingUsers(false);
        }
    };

    const fetchLogs = async () => {
        try {
            const response = await api.get('/sessionlogs');
            setSessionLogs(response.data);
        } catch (error) {
            console.error("Error fetching logs", error);
        }
    };

    const handleDelete = async (id) => {
        if (window.confirm('¿Estás seguro de eliminar este usuario?')) {
            await api.delete(`/users/${id}`);
            fetchUsers();
        }
    };

    return (
        <div className="space-y-6 animate-fade-in">
            {/* Header */}
            <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-semibold">Panel de Administración</h1>
                    <p className="text-sm text-[var(--text-muted)] mt-1">Gestiona usuarios y monitorea actividad</p>
                </div>
                <div className="flex gap-3">
                    <div className="flex bg-[var(--bg-secondary)] rounded-lg p-1 border border-[var(--border-secondary)]">
                        <button
                            onClick={() => setView('users')}
                            className={`px-4 py-1.5 text-sm font-medium rounded-md transition-all ${view === 'users'
                                ? 'bg-[var(--bg-card)] text-[var(--text-body)] shadow-sm'
                                : 'text-[var(--text-muted)] hover:text-[var(--text-body)]'
                                }`}
                        >
                            Usuarios
                        </button>
                        <button
                            onClick={() => setView('logs')}
                            className={`px-4 py-1.5 text-sm font-medium rounded-md transition-all ${view === 'logs'
                                ? 'bg-[var(--bg-card)] text-[var(--text-body)] shadow-sm'
                                : 'text-[var(--text-muted)] hover:text-[var(--text-body)]'
                                }`}
                        >
                            Actividad
                        </button>
                    </div>
                    {view === 'users' && (
                        <button
                            onClick={() => openModal('user')}
                            className="btn-primary"
                        >
                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4" />
                            </svg>
                            Nuevo Usuario
                        </button>
                    )}
                </div>
            </div>

            {/* Stats row */}
            <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
                <StatCard
                    label="Total Usuarios"
                    value={totalCount}
                    icon={
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                        </svg>
                    }
                />
                <StatCard
                    label="Administradores"
                    value={users.filter(u => u.role === 0).length}
                    icon={
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
                        </svg>
                    }
                />
            </div>

            {/* Content */}
            {view === 'users' ? (
                <div className="space-y-4">
                    {/* Search Bar */}
                    <div className="relative">
                        <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                            <svg className="h-5 w-5 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                            </svg>
                        </div>
                        <input
                            type="text"
                            className="input-base pl-10"
                            placeholder="Buscar usuarios por nombre, email..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                        />
                    </div>

                    {isLoadingUsers ? (
                        <div className="py-12 text-center">
                            <div className="w-8 h-8 border-2 border-accent-500 border-t-transparent rounded-full animate-spin mx-auto mb-2" />
                            <p className="text-[var(--text-muted)]">Cargando usuarios...</p>
                        </div>
                    ) : users.length > 0 ? (
                        <>
                            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
                                {users.map((u, index) => (
                                    <div
                                        key={u.id}
                                        className="card-interactive p-5 animate-slide-up"
                                        style={{ animationDelay: `${index * 50}ms` }}
                                    >
                                        <div className="flex items-start justify-between mb-4">
                                            <div className="flex items-center gap-3">
                                                <div className="w-10 h-10 rounded-full bg-[var(--bg-secondary)] flex items-center justify-center text-sm font-medium text-[var(--text-muted)]">
                                                    {u.firstName?.[0]}{u.lastName?.[0]}
                                                </div>
                                                <div>
                                                    <h3 className="font-medium">
                                                        {u.firstName} {u.lastName}
                                                    </h3>
                                                    <p className="text-sm text-[var(--text-muted)]">{u.email}</p>
                                                </div>
                                            </div>
                                            <span className={u.role === 0 ? 'badge-primary' : 'badge-secondary'}>
                                                {u.role === 0 ? 'Admin' : 'User'}
                                            </span>
                                        </div>

                                        <div className="flex gap-2 pt-4 border-t border-[var(--border-secondary)]">
                                            <button
                                                onClick={() => openModal('user', u)}
                                                className="btn-secondary flex-1 text-xs py-2"
                                            >
                                                <svg className="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                                                </svg>
                                                Editar
                                            </button>
                                            <button
                                                onClick={() => handleDelete(u.id)}
                                                className="btn-danger flex-1 text-xs py-2"
                                            >
                                                <svg className="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                                </svg>
                                                Eliminar
                                            </button>
                                        </div>
                                    </div>
                                ))}
                            </div>

                            {/* Pagination Controls */}
                            <div className="flex items-center justify-between border-t border-[var(--border-secondary)] pt-4">
                                <p className="text-sm text-[var(--text-muted)]">
                                    Página <span className="font-medium text-[var(--text-body)]">{page}</span> de <span className="font-medium text-[var(--text-body)]">{totalPages}</span>
                                </p>
                                <div className="flex gap-2">
                                    <button
                                        onClick={() => setPage(p => Math.max(1, p - 1))}
                                        disabled={page === 1}
                                        className="btn-secondary px-3 py-1 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
                                    >
                                        Anterior
                                    </button>
                                    <button
                                        onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                                        disabled={page === totalPages}
                                        className="btn-secondary px-3 py-1 text-sm disabled:opacity-50 disabled:cursor-not-allowed"
                                    >
                                        Siguiente
                                    </button>
                                </div>
                            </div>
                        </>
                    ) : (
                        <EmptyState
                            title="Sin resultados"
                            description={searchTerm ? `No se encontraron usuarios que coincidan con "${searchTerm}"` : "No hay usuarios registrados"}
                            action={() => openModal('user')}
                            actionLabel="Crear Usuario"
                        />
                    )}
                </div>
            ) : (
                <div className="card overflow-hidden">
                    <div className="overflow-x-auto">
                        <table className="w-full text-left text-sm">
                            <thead className="bg-[var(--bg-secondary)] text-[var(--text-muted)] border-b border-[var(--border-secondary)]">
                                <tr>
                                    <th className="px-6 py-3 font-medium">Usuario</th>
                                    <th className="px-6 py-3 font-medium">Acción</th>
                                    <th className="px-6 py-3 font-medium">IP</th>
                                    <th className="px-6 py-3 font-medium">Inicio</th>
                                    <th className="px-6 py-3 font-medium">Fin</th>
                                    <th className="px-6 py-3 font-medium">Duración</th>
                                </tr>
                            </thead>
                            <tbody className="divide-y divide-[var(--border-secondary)]">
                                {sessionLogs.map((log) => {
                                    const start = new Date(log.loginTime);
                                    const end = log.logoutTime ? new Date(log.logoutTime) : null;
                                    const duration = end ? Math.round((end - start) / 1000 / 60) + ' min' : '-';

                                    return (
                                        <tr key={log.id} className="hover:bg-[var(--bg-hover)] transition-colors">
                                            <td className="px-6 py-4">
                                                {log.user?.username || 'Usuario eliminado'}
                                            </td>
                                            <td className="px-6 py-4">
                                                <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium ${log.action === 'Login' ? 'bg-green-500/10 text-green-400' : 'bg-blue-500/10 text-blue-400'
                                                    }`}>
                                                    {log.action}
                                                </span>
                                            </td>
                                            <td className="px-6 py-4 text-[var(--text-muted)] font-mono text-xs">
                                                {log.ipAddress}
                                            </td>
                                            <td className="px-6 py-4 text-[var(--text-body)]">
                                                {start.toLocaleString()}
                                            </td>
                                            <td className="px-6 py-4 text-[var(--text-body)]">
                                                {end ? end.toLocaleString() : <span className="text-green-500 text-xs">En línea</span>}
                                            </td>
                                            <td className="px-6 py-4 text-[var(--text-muted)]">
                                                {duration}
                                            </td>
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                        {sessionLogs.length === 0 && (
                            <div className="p-8 text-center text-[var(--text-muted)]">
                                No hay registros de actividad
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

// ============================================
// User View
// ============================================
const UserView = ({ profile, refresh, openModal }) => {
    if (!profile) return null;

    const handleDeleteStudy = async (id) => {
        if (window.confirm('¿Estás seguro de eliminar este estudio?')) {
            try {
                await api.delete(`/studies/${id}`);
                refresh();
            } catch (error) {
                console.error("Error deleting study:", error);
                alert("Error al eliminar el estudio");
            }
        }
    };

    return (
        <div className="max-w-2xl mx-auto space-y-6 animate-fade-in">
            {/* Profile Card */}
            <div className="card p-6">
                <div className="flex items-center justify-between mb-6 pb-4 border-b border-[var(--border-secondary)]">
                    <h2 className="text-lg font-semibold">Mi Perfil</h2>
                    <button
                        onClick={() => openModal('user', profile)}
                        className="btn-ghost text-sm"
                    >
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                        </svg>
                        Editar
                    </button>
                </div>

                <div className="flex items-center gap-4 mb-6">
                    <div className="w-16 h-16 rounded-full bg-accent-600/20 border border-accent-500/30 flex items-center justify-center">
                        <span className="text-xl font-semibold text-accent-400">
                            {profile.firstName?.[0]}{profile.lastName?.[0]}
                        </span>
                    </div>
                    <div>
                        <h3 className="text-xl font-semibold">
                            {profile.firstName} {profile.lastName}
                        </h3>
                        <p className="text-[var(--text-muted)]">{profile.email}</p>
                    </div>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div className="p-4 rounded-xl bg-[var(--bg-secondary)] border border-[var(--border-secondary)]">
                        <span className="label-uppercase">Nombre</span>
                        <p className="font-medium">{profile.firstName}</p>
                    </div>
                    <div className="p-4 rounded-xl bg-[var(--bg-secondary)] border border-[var(--border-secondary)]">
                        <span className="label-uppercase">Apellido</span>
                        <p className="font-medium">{profile.lastName}</p>
                    </div>
                </div>
            </div>

            {/* Studies Card */}
            <div className="card p-6">
                <div className="flex items-center justify-between mb-4">
                    <div className="flex items-center gap-2">
                        <svg className="w-5 h-5 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M12 14l9-5-9-5-9 5 9 5z" />
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M12 14l6.16-3.422a12.083 12.083 0 01.665 6.479A11.952 11.952 0 0012 20.055a11.952 11.952 0 00-6.824-2.998 12.078 12.078 0 01.665-6.479L12 14z" />
                        </svg>
                        <h2 className="text-lg font-semibold">Mis Estudios</h2>
                    </div>
                    <button
                        onClick={() => openModal('study')}
                        className="btn-ghost text-accent-400 hover:text-accent-300 text-sm"
                    >
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4" />
                        </svg>
                        Agregar
                    </button>
                </div>

                <div className="space-y-3">
                    {profile.studies?.length > 0 ? (
                        profile.studies.map((s, index) => (
                            <div
                                key={s.id}
                                className="group flex items-center justify-between p-4 rounded-xl bg-[var(--bg-secondary)] border border-[var(--border-secondary)] hover:border-[var(--border-card)] transition-all"
                            >
                                <div>
                                    <h4 className="font-medium">{s.title}</h4>
                                    <p className="text-sm text-[var(--text-muted)]">
                                        {s.description}
                                    </p>
                                </div>
                                <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                                    <button
                                        onClick={() => openModal('study', s)}
                                        className="btn-ghost p-2 text-gray-400 hover:text-white"
                                        title="Editar"
                                    >
                                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                                        </svg>
                                    </button>
                                    <button
                                        onClick={() => handleDeleteStudy(s.id)}
                                        className="btn-ghost p-2 text-red-400 hover:text-red-300 hover:bg-red-500/10"
                                        title="Eliminar"
                                    >
                                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                        </svg>
                                    </button>
                                </div>
                            </div>
                        ))
                    ) : (
                        <div className="text-center py-8 text-[var(--text-muted)]">
                            <svg className="w-12 h-12 mx-auto mb-3 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1" d="M12 14l9-5-9-5-9 5 9 5z" />
                            </svg>
                            <p className="text-sm">No hay estudios registrados</p>
                        </div>
                    )}
                </div>
            </div>

            {/* Address Card */}
            <div className="card p-6">
                <div className="flex items-center justify-between mb-4">
                    <div className="flex items-center gap-2">
                        <svg className="w-5 h-5 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                        </svg>
                        <h2 className="text-lg font-semibold">Mi Dirección</h2>
                    </div>
                    {!profile.address && (
                        <button
                            onClick={() => openModal('address')}
                            className="btn-ghost text-accent-400 hover:text-accent-300 text-sm"
                        >
                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4v16m8-8H4" />
                            </svg>
                            Configurar
                        </button>
                    )}
                </div>

                {profile.address ? (
                    <div className="group flex items-center justify-between p-4 rounded-xl bg-[var(--bg-secondary)] border border-[var(--border-secondary)] hover:border-[var(--border-card)] transition-all">
                        <div>
                            <p className="font-medium">
                                {profile.address.street}
                            </p>
                            <p className="text-sm text-[var(--text-muted)]">
                                {profile.address.city}, {profile.address.country} {profile.address.zipCode && `(${profile.address.zipCode})`}
                            </p>
                        </div>
                        <button
                            onClick={() => openModal('address', profile.address)}
                            className="btn-ghost p-2 opacity-0 group-hover:opacity-100 transition-opacity"
                        >
                            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" />
                            </svg>
                        </button>
                    </div>
                ) : (
                    <div className="text-center py-8 text-[var(--text-muted)]">
                        <svg className="w-12 h-12 mx-auto mb-3 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                        </svg>
                        <p className="text-sm">No hay dirección registrada</p>
                    </div>
                )}
            </div>
        </div>
    );
};

// ============================================
// Shared Components
// ============================================
const StatCard = ({ label, value, icon }) => (
    <div className="card p-4">
        <div className="flex items-center justify-between">
            <div>
                <p className="text-sm text-[var(--text-muted)]">{label}</p>
                <p className="text-2xl font-semibold mt-1">{value}</p>
            </div>
            <div className="w-10 h-10 rounded-xl bg-[var(--bg-secondary)] flex items-center justify-center text-[var(--text-muted)]">
                {icon}
            </div>
        </div>
    </div>
);

const EmptyState = ({ title, description, action, actionLabel }) => (
    <div className="card p-12 text-center">
        <div className="w-16 h-16 rounded-2xl bg-[var(--bg-secondary)] flex items-center justify-center mx-auto mb-4">
            <svg className="w-8 h-8 text-[var(--text-muted)]" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z" />
            </svg>
        </div>
        <h3 className="text-lg font-medium mb-1">{title}</h3>
        <p className="text-[var(--text-muted)] text-sm mb-6">{description}</p>
        {action && (
            <button onClick={action} className="btn-primary">
                {actionLabel}
            </button>
        )}
    </div>
);

// ============================================
// Forms
// ============================================
const StudyForm = ({ item, userId, onSuccess }) => {
    const [title, setTitle] = useState(item?.title || '');
    const [description, setDescription] = useState(item?.description || '');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        try {
            const data = { title, description, userId: parseInt(userId) };
            if (item) await api.put(`/studies/${item.id}`, data);
            else await api.post('/studies', data);
            onSuccess();
        } catch (error) {
            console.error("Error saving study:", error);
            alert("Error al guardar el estudio. Por favor intente nuevamente.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <label htmlFor="study-title" className="label">Título del estudio</label>
                <input
                    id="study-title"
                    className="input-base"
                    placeholder="Ej: Licenciatura en Informática"
                    value={title}
                    onChange={e => setTitle(e.target.value)}
                    required
                    disabled={isSubmitting}
                />
            </div>
            <div>
                <label htmlFor="study-description" className="label">Descripción</label>
                <textarea
                    id="study-description"
                    className="input-base min-h-[100px] resize-none"
                    placeholder="Ej: Universidad Nacional, 2023"
                    value={description}
                    onChange={e => setDescription(e.target.value)}
                    disabled={isSubmitting}
                />
            </div>
            <button
                type="submit"
                disabled={isSubmitting}
                className="btn-primary w-full py-3 mt-2"
            >
                {isSubmitting ? 'Guardando...' : 'Guardar'}
            </button>
        </form>
    );
};

const AddressForm = ({ item, userId, onSuccess }) => {
    const [street, setStreet] = useState(item?.street || '');
    const [city, setCity] = useState(item?.city || '');
    const [state, setState] = useState(item?.state || '');
    const [zipCode, setZipCode] = useState(item?.zipCode || '');
    const [country, setCountry] = useState(item?.country || '');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        try {
            const data = { street, city, state, zipCode, country, userId: parseInt(userId) };
            if (item) await api.put(`/addresses/${item.id}`, data);
            else await api.post('/addresses', data);
            onSuccess();
        } catch (error) {
            console.error("Error saving address:", error);
            alert("Error al guardar la dirección. Por favor intente nuevamente.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <label htmlFor="address-street" className="label">Calle y número</label>
                <input
                    id="address-street"
                    className="input-base"
                    placeholder="Ej: Av. Corrientes 1234"
                    value={street}
                    onChange={e => setStreet(e.target.value)}
                    required
                    disabled={isSubmitting}
                />
            </div>
            <div className="grid grid-cols-2 gap-3">
                <div>
                    <label htmlFor="address-city" className="label">Ciudad</label>
                    <input
                        id="address-city"
                        className="input-base"
                        placeholder="Ej: Buenos Aires"
                        value={city}
                        onChange={e => setCity(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
                <div>
                    <label htmlFor="address-state" className="label">Provincia/Estado</label>
                    <input
                        id="address-state"
                        className="input-base"
                        placeholder="Ej: CABA"
                        value={state}
                        onChange={e => setState(e.target.value)}
                        disabled={isSubmitting}
                    />
                </div>
            </div>
            <div className="grid grid-cols-2 gap-3">
                <div>
                    <label htmlFor="address-zipCode" className="label">Código Postal</label>
                    <input
                        id="address-zipCode"
                        className="input-base"
                        placeholder="Ej: 1000"
                        value={zipCode}
                        onChange={e => setZipCode(e.target.value)}
                        disabled={isSubmitting}
                    />
                </div>
                <div>
                    <label htmlFor="address-country" className="label">País</label>
                    <input
                        id="address-country"
                        className="input-base"
                        placeholder="Ej: Argentina"
                        value={country}
                        onChange={e => setCountry(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
            </div>
            <button
                type="submit"
                disabled={isSubmitting}
                className="btn-primary w-full py-3 mt-2"
            >
                {isSubmitting ? 'Guardando...' : 'Guardar'}
            </button>
        </form>
    );
};

const UserForm = ({ item, onSuccess }) => {
    const [username, setUsername] = useState(item?.username || '');
    const [email, setEmail] = useState(item?.email || '');
    const [firstName, setFirstName] = useState(item?.firstName || '');
    const [lastName, setLastName] = useState(item?.lastName || '');
    const [role, setRole] = useState(item?.role ?? 1);
    const [password, setPassword] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        try {
            const data = { username, email, firstName, lastName, role: parseInt(role) };
            if (password) data.password = password;

            if (item) await api.put(`/users/${item.id}`, data);
            else await api.post('/users', data);
            onSuccess();
        } catch (error) {
            console.error("Error saving user:", error);
            alert("Error al guardar el usuario. Por favor intente nuevamente.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-2 gap-3">
                <div>
                    <label htmlFor="user-firstName" className="label">Nombre</label>
                    <input
                        id="user-firstName"
                        className="input-base"
                        placeholder="Juan"
                        value={firstName}
                        onChange={e => setFirstName(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
                <div>
                    <label htmlFor="user-lastName" className="label">Apellido</label>
                    <input
                        id="user-lastName"
                        className="input-base"
                        placeholder="Pérez"
                        value={lastName}
                        onChange={e => setLastName(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
            </div>
            <div className="grid grid-cols-2 gap-3">
                <div>
                    <label htmlFor="user-username" className="label">Usuario</label>
                    <input
                        id="user-username"
                        className="input-base"
                        placeholder="juanperez"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
                <div>
                    <label htmlFor="user-role" className="label">Rol</label>
                    <select
                        id="user-role"
                        className="input-base"
                        value={role}
                        onChange={e => setRole(e.target.value)}
                        disabled={isSubmitting}
                    >
                        <option value={0}>Administrador</option>
                        <option value={1}>Usuario</option>
                    </select>
                </div>
            </div>
            <div>
                <label htmlFor="user-email" className="label">Email</label>
                <input
                    id="user-email"
                    type="email"
                    className="input-base"
                    placeholder="juan@ejemplo.com"
                    value={email}
                    onChange={e => setEmail(e.target.value)}
                    required
                    disabled={isSubmitting}
                />
            </div>
            {!item && (
                <div>
                    <label htmlFor="user-password" className="label">Contraseña</label>
                    <input
                        id="user-password"
                        type="password"
                        className="input-base"
                        placeholder="••••••••"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        required
                        disabled={isSubmitting}
                    />
                </div>
            )}
            <button
                type="submit"
                disabled={isSubmitting}
                className="btn-primary w-full py-3 mt-2"
            >
                {isSubmitting ? 'Guardando...' : 'Guardar'}
            </button>
        </form>
    );
};

export default Dashboard;
