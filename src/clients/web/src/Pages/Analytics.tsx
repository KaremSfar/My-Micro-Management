import WeeklyScheduleTable from "../Components/WeeklyScheduleTable";

function Analytics() {
    // Example events - replace with your actual events
    const events = [
        {
            id: 1,
            title: "Meeting",
            start: new Date('2024-01-01T12:33:00'),
            end: new Date('2024-01-01T14:15:00'),
        },
        {
            id: 2,
            title: "Call",
            start: new Date('2024-01-02T09:45:00'),
            end: new Date('2024-01-02T10:30:00'),
        }
    ];

    return (
        <div className="w-full h-full flex items-center justify-center my-16">
            <div className="w-full h-full bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden">
                {/* Header */}
                <div className="px-6 py-4 border-b border-gray-200 bg-gray-50">
                    <h2 className="text-xl font-semibold text-gray-800">Weekly Schedule</h2>
                    <p className="text-sm text-gray-600 mt-1">View your events across the week</p>
                </div>

                {/* Table Container */}
                <div className="p-6">
                    <WeeklyScheduleTable events={events} />
                </div>
            </div>
        </div>
    );
}

export default Analytics;