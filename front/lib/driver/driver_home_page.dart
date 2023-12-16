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
      body: const Placeholder(),
    );
  }
}
