import React, { useState } from 'react';
import { useContextContext } from '../context/ContextContext';
import { contextIconOptions, resolveContextIcon } from '../utils/iconResolver';

interface NewContextFormProps {
    onClose: () => void;
}

function NewContextForm({ onClose }: NewContextFormProps) {
    const { createNewContext } = useContextContext();
    const [contextName, setContextName] = useState('');
    const [iconName, setIconName] = useState(contextIconOptions[0].value);

    const SelectedIcon = resolveContextIcon(iconName);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        try {
            await createNewContext({ name: contextName, icon: iconName });
            onClose();
        } catch (error) {
            console.error('Error creating context:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="space-y-4">
            <div>
                <label htmlFor="contextName" className="block text-sm font-medium text-gray-700">
                    Context Name
                </label>
                <input
                    type="text"
                    id="contextName"
                    value={contextName}
                    onChange={(e) => setContextName(e.target.value)}
                    required
                    className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-300 focus:ring focus:ring-indigo-200 focus:ring-opacity-50"
                />
            </div>
            <div>
                <label htmlFor="contextIcon" className="block text-sm font-medium text-gray-700">
                    Icon
                </label>
                <div className="mt-1 flex items-center gap-2 rounded-md border border-gray-300 px-3 py-2">
                    <SelectedIcon className="w-4 h-4 text-slate-600" />
                    <select
                        id="contextIcon"
                        value={iconName}
                        onChange={(e) => setIconName(e.target.value)}
                        className="w-full bg-transparent focus:outline-none"
                    >
                        {contextIconOptions.map((iconOption) => (
                            <option key={iconOption.value} value={iconOption.value}>{iconOption.label}</option>
                        ))}
                    </select>
                </div>
            </div>
            <div className="flex justify-end space-x-2">
                <button
                    type="button"
                    onClick={onClose}
                    className="px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                    Cancel
                </button>
                <button
                    type="submit"
                    className="px-4 py-2 text-sm font-medium text-white bg-indigo-600 border border-transparent rounded-md hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                    Create Context
                </button>
            </div>
        </form>
    );
}

export default NewContextForm;
