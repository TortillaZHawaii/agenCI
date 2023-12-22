import 'package:flutter/material.dart';

class ParkingHomePage extends StatelessWidget {
  const ParkingHomePage({
    super.key,
    required this.parkingId,
  });

  final String parkingId;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Parking $parkingId"),
      ),
      body: const Placeholder(),
    );
  }
}
