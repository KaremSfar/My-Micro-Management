import { useContextContext } from '../context/ContextContext';
import { resolveContextIcon } from '../utils/iconResolver';

const ContextIsland = () => {
    const { contexts, selectedContextId, setSelectedContextId } = useContextContext();

    if (!contexts.length) {
        return null;
    }

    return (
        <div className="flex items-center gap-1 border-2 border-slate-200 rounded-xl bg-white shadow-sm px-2 py-1">
            {contexts.map((context) => {
                const Icon = resolveContextIcon(context.icon);
                const isActive = selectedContextId === context.id;

                return (
                    <button
                        key={context.id}
                        type="button"
                        title={context.name}
                        onClick={() => setSelectedContextId(isActive ? null : context.id)}
                        className={`w-9 h-9 rounded-lg flex items-center justify-center transition-colors ${
                            isActive ? 'bg-zinc-100' : 'hover:bg-slate-50'
                        }`}
                    >
                        <Icon className="w-4 h-4 text-slate-600" />
                    </button>
                );
            })}
        </div>
    );
};

export default ContextIsland;
