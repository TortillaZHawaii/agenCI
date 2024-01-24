import 'package:agenci/driver/select_offer_page.dart';
import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

class SelectRangePage extends StatefulWidget {
  const SelectRangePage({super.key, required this.driverId});
  final String driverId;

  @override
  State<SelectRangePage> createState() => _SelectRangePageState();
}

class _SelectRangePageState extends State<SelectRangePage> {
  late GoogleMapController mapController;
  final _center = LatLng(52.2195, 21.0112);
  final _zoom = 16.0;

  void _onMapCreated(GoogleMapController controller) {
    mapController = controller;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Select Parking Time and Place"),
      ),
      body: Column(
        children: [
          Expanded(
            child: Placeholder(
                child: Stack(
              alignment: Alignment.center,
              children: [
                Center(
                  child: GoogleMap(
                    onMapCreated: _onMapCreated,
                    initialCameraPosition: CameraPosition(
                      target: _center,
                      zoom: _zoom,
                    ),
                  ),
                ),
                Icon(
                  Icons.control_point,
                  size: 40
                )
              ]
            ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8),
            child: ElevatedButton(
              onPressed: () async {
                final startDate = await showDatePicker(
                  context: context,
                  initialDate: DateTime.now(),
                  firstDate: DateTime.now(),
                  lastDate: DateTime.now().add(const Duration(days: 7)),
                );

                if (startDate == null) {
                  return;
                }

                final startTime = await showTimePicker(
                  context: context,
                  initialTime: TimeOfDay.now(),
                );

                if (startTime == null) {
                  return;
                }

                final endDate = await showDatePicker(
                  context: context,
                  initialDate: startDate,
                  firstDate: startDate,
                  lastDate: DateTime.now().add(const Duration(days: 7)),
                );

                if (endDate == null) {
                  return;
                }

                final endTime = await showTimePicker(
                  context: context,
                  initialTime: startTime,
                );

                if (endTime == null) {
                  return;
                }

                final start = DateTime(
                  startDate.year,
                  startDate.month,
                  startDate.day,
                  startTime.hour,
                  startTime.minute,
                );

                final end = DateTime(
                  endDate.year,
                  endDate.month,
                  endDate.day,
                  endTime.hour,
                  endTime.minute,
                );

                // TODO: Get latitude and longitude from map
                double screenWidth = MediaQuery.of(context).size.width *
                    MediaQuery.of(context).devicePixelRatio;
                double screenHeight = MediaQuery.of(context).size.height *
                    MediaQuery.of(context).devicePixelRatio;

                double middleX = screenWidth / 2;
                double middleY = screenHeight / 2;

                var screenCoordinate = ScreenCoordinate(x: middleX.round(), y: middleY.round());
                LatLng latLng = await mapController.getLatLng(screenCoordinate);

                final latitude = latLng.latitude;
                final longitude = latLng.longitude;

                await Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => SelectOfferPage(
                      driverId: widget.driverId,
                      start: start,
                      end: end,
                      latitude: latitude,
                      longitude: longitude,
                    ),
                  ),
                );
              },
              child: const Text("Search for Offers"),
            ),
          ),
        ],
      ),
    );
  }
}
