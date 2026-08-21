using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automationassignment_01.Models;
public sealed class SearchHotelData
{
    public string Id { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Location { get; init; } = string.Empty;

    public string Hotel { get; init; } = string.Empty;

    public string RoomType { get; init; } = string.Empty;

    public string NumberOfRooms { get; init; } = string.Empty;

    public int CheckInOffsetDays { get; init; }

    public int CheckOutOffsetDays { get; init; }

    public string AdultsPerRoom { get; init; } = string.Empty;

    public string ChildrenPerRoom { get; init; } = string.Empty;
    public string expectedhotelname { get; init; } = string.Empty;

    public string expectedlocation { get; init; } =  string.Empty;
    public string ExpectedMessage { get; init; } = string.Empty;
}