export interface TimeTableEvent {
    title: string;
    start: Date;
    end: Date;
    color: string;
}

interface WeeklyScheduleTableProps {
    events: TimeTableEvent[];
}

function WeeklyScheduleTable({ events }: WeeklyScheduleTableProps) {
    const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const hours = Array.from({ length: 24 }, (_, i) => i);

    const getEventStyle = (event: TimeTableEvent) => {
        const startHour = event.start.getHours();
        const startMinute = event.start.getMinutes();
        const endHour = event.end.getHours();
        const endMinute = event.end.getMinutes();

        const startDecimal = startHour + (startMinute / 60);
        let endDecimal = endHour + (endMinute / 60);
        
        // Cap at 24 hours (midnight) to prevent overflow
        if (endDecimal > 24 || (endHour === 23 && endMinute === 59)) {
            endDecimal = 24;
        }

        // Calculate position as percentage (0-100%)
        const left = (startDecimal / 24) * 100;
        const width = ((endDecimal - startDecimal) / 24) * 100;

        return {
            left: `${left}%`,
            width: `${width}%`,
            backgroundColor: event.color
        };
    };

    function getLastSunday(): Date {
        const today = new Date();
        const dayOfWeek = today.getDay(); // Sunday = 0, Monday = 1, ..., Saturday = 6

        // Calculate the difference in days to the last Sunday
        const diff = today.getDate() - dayOfWeek;

        const lastSunday = new Date(today.setDate(diff));

        // Reset time to midnight
        lastSunday.setHours(0, 0, 0, 0);

        return lastSunday;
    }

    const lastSunday = getLastSunday();

    // Split events that cross midnight into two events
    const splitMultiDayEvents = (events: TimeTableEvent[]): TimeTableEvent[] => {
        const result: TimeTableEvent[] = [];
        
        events.forEach(event => {
            const startDate = new Date(event.start);
            const endDate = new Date(event.end);
            
            // Check if event crosses midnight
            if (startDate.getDate() !== endDate.getDate()) {
                // First day: from start time to midnight
                const firstDayEnd = new Date(startDate);
                firstDayEnd.setHours(23, 59, 59, 999);
                
                result.push({
                    ...event,
                    end: firstDayEnd
                });
                
                // Next day: from midnight to end time
                const nextDayStart = new Date(endDate);
                nextDayStart.setHours(0, 0, 0, 0);
                
                result.push({
                    ...event,
                    start: nextDayStart,
                    end: endDate
                });
            } else {
                // Event is within a single day
                result.push(event);
            }
        });
        
        return result;
    };

    const splitEvents = splitMultiDayEvents(events);
    const filteredEvents = splitEvents.filter(e => e.start >= lastSunday);

    // Calculate the next Sunday to filter events within the current week
    const nextSunday = new Date(lastSunday);
    nextSunday.setDate(nextSunday.getDate() + 7);

    const showThisDaysEvents = (dayIndexOnTable: number) => {
        return (event: TimeTableEvent) => {
            // Check if event is on the correct day of week AND within the current week
            return event.start.getDay() === dayIndexOnTable && 
                   event.start >= lastSunday && 
                   event.start < nextSunday;
        };
    };

    return (
        <div className="w-full h-full flex flex-col">
            {/* Header with hour markers */}
            <div className="flex items-center mb-2 h-6">
                <div className="w-12 flex-shrink-0"></div>
                <div className="flex-1 relative h-full">
                    {[0, 6, 12, 18, 24].map((hour) => (
                        <div
                            key={hour}
                            className="absolute text-xs text-gray-500 -translate-x-1/2"
                            style={{ left: `${(hour / 24) * 100}%` }}
                        >
                            {hour}:00
                        </div>
                    ))}
                </div>
            </div>

            {/* Days grid */}
            <div className="flex-1 flex flex-col border-t border-gray-300 overflow-y-auto">
                {days.map((day, dayIndex) => (
                    <div key={day} className="flex h-[40px] border-b border-gray-300">
                        {/* Day label */}
                        <div className="w-12 flex-shrink-0 text-gray-600 pr-2 font-medium text-sm border-r border-gray-300 flex items-center">
                            {day}
                        </div>

                        {/* Timeline container */}
                        <div className="flex-1 relative">
                            {/* Grid cells background */}
                            <div className="absolute inset-0 flex">
                                {hours.map((hour) => (
                                    <div
                                        key={hour}
                                        className="flex-1 border-r border-gray-200"
                                    />
                                ))}
                            </div>

                            {/* Events overlay */}
                            <div className="absolute inset-0 overflow-hidden">
                                {filteredEvents
                                    .filter(showThisDaysEvents(dayIndex))
                                    .map((event, i) => {
                                        const style = getEventStyle(event);
                                        const width = parseFloat(style.width);
                                        const left = parseFloat(style.left);
                                        
                                        // Ensure event doesn't overflow the container
                                        const adjustedWidth = Math.min(width, 100 - left);
                                        
                                        return (
                                            <div
                                                key={i}
                                                className="group absolute text-white text-xs px-2 rounded opacity-90 hover:opacity-100 cursor-pointer flex items-center overflow-hidden"
                                                style={{
                                                    left: `${left}%`,
                                                    width: `${adjustedWidth}%`,
                                                    backgroundColor: style.backgroundColor,
                                                    top: '4px',
                                                    height: 'calc(100% - 8px)',
                                                    maxWidth: `calc(100% - ${left}%)`
                                                }}
                                                title={`${event.title} (${event.start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - ${event.end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })})`}
                                            >
                                                <span className="truncate font-medium">
                                                    {event.title}
                                                </span>

                                                {/* Hover tooltip */}
                                                <div className="hidden group-hover:block absolute bottom-full left-1/2 -translate-x-1/2 mb-2 w-max bg-gray-800 text-white px-3 py-2 rounded-md text-sm shadow-lg z-50">
                                                    <div className="font-semibold">{event.title}</div>
                                                    <div className="text-gray-300 text-xs mt-1">
                                                        {event.start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - {event.end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                                    </div>
                                                    <div className="absolute top-full left-1/2 -translate-x-1/2 w-0 h-0 border-x-4 border-x-transparent border-t-4 border-t-gray-800"></div>
                                                </div>
                                            </div>
                                        );
                                    })
                                }
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default WeeklyScheduleTable;