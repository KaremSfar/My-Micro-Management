import WeeklyScheduleTable, { TimeTableEvent } from "../Components/WeeklyScheduleTable";
import { useTimeSessions } from "../hooks/useTimeSessions";

function Analytics() {
    const { timeSessions, loading } = useTimeSessions();
    const events: TimeTableEvent[] = timeSessions.map(ts => {
        return {
            start: ts.startTime,
            end: ts.endTime ?? new Date(),
            title: ts.project.name,
            color: ts.project.color
        }
    }).filter(ts => (ts.end.getTime() - ts.start.getTime()) > 60 * 1000);

    if (loading) {
        return (
            <div className="flex items-center justify-center my-8">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto mb-4"></div>
                    <p className="text-gray-600 font-medium">Loading your schedule...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="h-full w-full flex flex-col p-8">
            <div className="flex-1 w-full max-w-6xl mx-auto bg-white rounded-xl shadow-lg border border-gray-200 overflow-hidden flex flex-col min-h-0">
                {/* Header */}
                <div className="px-6 py-4 border-b border-gray-200 bg-gray-50 flex-shrink-0">
                    <h2 className="text-xl font-semibold text-gray-800">Weekly Schedule</h2>
                    <p className="text-sm text-gray-600 mt-1">View your events across the week</p>
                </div>

                {/* Table Container */}
                <div className="flex-1 p-6 overflow-auto min-h-0">
                    <WeeklyScheduleTable events={events} />
                </div>
            </div>
        </div>
    );
}

export default Analytics;