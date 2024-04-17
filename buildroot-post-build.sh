#!/bin/bash -e

# The init scripts used to auto-expand the persistent rootfs on the first boot
# to fill the medium must not be used with the volatile (initramfs) rootfs.
if grep "^BR2_TARGET_ROOTFS_INITRAMFS=y$" "${BR2_CONFIG}" &>/dev/null; then
	rm -f "${TARGET_DIR}"/etc/init.d/{S22expand-rootpart,S23expand-rootfs}
fi
