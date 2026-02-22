import { useContextContext } from '../context/ContextContext';
import { resolveContextIcon } from '../utils/iconResolver';
import { Dialog } from '@headlessui/react';
import { PlusIcon } from '@heroicons/react/24/outline';
import { useState } from 'react';
import NewContextForm from './NewContextForm';

const ContextIsland = () => {
    const { contexts, selectedContextId, setSelectedContextId } = useContextContext();
    const [isCreateContextModalOpen, setIsCreateContextModalOpen] = useState(false);

    return (
        <>
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

                <button
                    type="button"
                    title="Add context"
                    onClick={() => setIsCreateContextModalOpen(true)}
                    className="w-9 h-9 rounded-lg flex items-center justify-center transition-colors hover:bg-slate-50"
                >
                    <PlusIcon className="w-4 h-4 text-slate-600" />
                </button>
            </div>

            <Dialog open={isCreateContextModalOpen} onClose={() => setIsCreateContextModalOpen(false)}>
                <Dialog.Overlay className="fixed inset-0 bg-black opacity-30" />
                <div className="fixed inset-0 flex items-center justify-center">
                    <Dialog.Panel className="bg-white rounded p-6 w-full max-w-md">
                        <Dialog.Title className="text-lg font-medium mb-4">Create New Context</Dialog.Title>
                        <NewContextForm onClose={() => setIsCreateContextModalOpen(false)} />
                    </Dialog.Panel>
                </div>
            </Dialog>
        </>
    );
};

export default ContextIsland;
