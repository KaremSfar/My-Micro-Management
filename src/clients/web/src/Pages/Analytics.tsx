import WeeklyScheduleTable, { TimeTableEvent } from "../Components/WeeklyScheduleTable";
import { useTimeSessions } from "../hooks/useTimeSessions";

function Analytics() {
    const timeSessions = useTimeSessions();
    const events: TimeTableEvent[] = timeSessions.timeSessions.map(ts => {
        return {
            start: ts.startTime,
            end: ts.endTime ?? new Date(),
            title: ts.project.name,
            color: ts.project.color
        }
    }).filter(ts => (ts.end.getTime() - ts.start.getTime()) > 60 * 1000);

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