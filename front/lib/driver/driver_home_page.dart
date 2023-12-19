import 'package:flutter/material.dart';

class DriverHomePage extends StatelessWidget {
  const DriverHomePage({
    super.key,
    required this.driverId,
  });

  final String driverId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text("Driver $driverId"),
        ),
        body: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              children: [
                Card(
                  child: InkWell(
                    onTap: () {
                      //
                    },
                    child: const ListTile(
                      leading: Icon(Icons.directions_car),
                      title: Text("Select Parking Time and Place"),
                      subtitle: Text("Pick and choose from available offers"),
                    ),
                  ),
                ),
                Card(
                  child: InkWell(
                    onTap: () {
                      //
                    },
                    child: const ListTile(
                      leading: Icon(Icons.history),
                      title: Text("View History"),
                      subtitle: Text("View your parking history"),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ));
  }
}
