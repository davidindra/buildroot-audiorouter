FROM davidindracz/buildroot-base:latest AS build

WORKDIR /buildroot/

# this .config was obtained by running `make raspberrypi3_defconfig` and manual modifications by `make menuconfig`
COPY buildroot.config .config

COPY buildroot-post-build.sh post-build.sh
RUN chmod u+x ./post-build.sh

# setup overlay

WORKDIR /buildroot-overlay/

COPY buildroot-overlay ./

RUN chmod 644 ./etc/dhcp/dhcpd.conf

RUN chmod u+x ./etc/init.d/S02modules
RUN chmod u+x ./etc/init.d/S02procps
RUN chmod u+x ./etc/init.d/S22expand-rootpart
RUN chmod u+x ./etc/init.d/S23expand-rootfs
RUN chmod u+x ./etc/init.d/S99firewall

RUN chmod 644 ./etc/modprobe.d/focusrite-scarlett.conf

RUN chmod 644 ./etc/network/interfaces
RUN chmod 644 ./etc/ssh/sshd_config

RUN chmod u+x ./etc/sysconfig/functions

RUN chmod 644 ./etc/sysctl.conf
RUN chmod 644 ./etc/wpa_supplicant.conf

# build the image

WORKDIR /buildroot/

RUN make all

# retrieve image
FROM scratch AS export

COPY --from=build /buildroot/output/images/sdcard.img .

ENTRYPOINT [ "/usr/bin/echo", "This image is not meant to be run. The resulting buildroot image is being produced as part of the docker build step." ]

# build: docker build -t buildroot-audiorouter --output out .

# manage config: docker run --pull always --name buildroot-cfg -v "$(pwd):/mnt/git" -itd davidindracz/buildroot-base:latest
#                cp /mnt/git/buildroot.config /buildroot/.config
#                make menuconfig
#                cp /buildroot/.config /mnt/git/buildroot.config