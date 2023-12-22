import 'package:agenci/driver/select_offer_page.dart';
import 'package:flutter/material.dart';

class SelectRangePage extends StatelessWidget {
  const SelectRangePage({super.key, required this.driverId});

  final String driverId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Select Parking Time and Place"),
      ),
      body: Column(
        children: [
          const Expanded(
            child: Placeholder(
              child: Center(
                child: Text("Map goes here"),
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
                final latitude = 0.0;
                final longitude = 0.0;

                await Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => SelectOfferPage(
                      driverId: driverId,
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
