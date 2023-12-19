import 'package:flutter/material.dart';

class SelectOfferPage extends StatelessWidget {
  const SelectOfferPage({
    super.key,
    required this.driverId,
    required this.start,
    required this.end,
    required this.latitude,
    required this.longitude,
  });

  final String driverId;
  final DateTime start;
  final DateTime end;
  final double latitude;
  final double longitude;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Select Offer"),
      ),
      body: const Placeholder(),
    );
  }
}
